using Actor.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actor.Server;
using Newtonsoft.Json;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using System.Collections.ObjectModel;
using System.Threading;
using System.Reactive.Disposables;
using System.Reactive.PlatformServices;
using System.Reactive.Threading;
using System.Reactive.Concurrency;

namespace ARnEdSpy.MRActor
{
    public enum MRQuery { None, AddData, FindData, RxData} ;


    public class actObservable<T> : actActor , IObservable<T>
    {
        public static Lazy<actObservable<T>> instance = new Lazy<actObservable<T>>();
        private List<IObserver<T>> observers;
        public actObservable()
        {
            observers = new List<IObserver<T>>();
            // var bhv1 = new bhvBehavior<IObserver<T>>(DoSubscribe);
            var bhv2 = new bhvBehavior<T>(DoTrack);
            // var bhv3 = new bhvBehavior(DoEndTransmission);

            // Become(bhv1);
            Become(bhv2);
        }

        public IDisposable Subscribe(IObserver<T> observer)
        {
            //SendMessage(observer);
            //Task<Object> res = Receive(t => { return t is Object; }).ContinueWith(t => { return t.Result; });
            //var resi =  res.Result as IDisposable ;
            //return resi;
            if (!observers.Contains(observer))
                observers.Add(observer);
            IDisposable dispo = new Unsubscriber(observers, observer);
            return dispo;
            // var act = new actEcho();
            // act.SendMessage(new Tuple<IActor, Object>(this, dispo));
        }

        public void Track(T loc)
        {
            SendMessage(loc);
        }

        private void DoTrack(T loc)
        {
            foreach (var observer in observers)
            {
                if (loc == null)
                    observer.OnError(new Exception());
                else
                    observer.OnNext(loc);
            }
        }

        private void DoEndTransmission()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();

            observers.Clear();
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<T>> _observers;
            private IObserver<T> _observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }
    }


    public class LocationTracker<T> : IObservable<T>
    {
        public LocationTracker()
        {
            observers = new List<IObserver<T>>();
        }

        private List<IObserver<T>> observers;

        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!observers.Contains(observer))
                observers.Add(observer);
            return new Unsubscriber(observers, observer);
        }

        private class Unsubscriber : IDisposable
        {
            private List<IObserver<T>> _observers;
            private IObserver<T> _observer;

            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }

            public void Dispose()
            {
                if (_observer != null && _observers.Contains(_observer))
                    _observers.Remove(_observer);
            }
        }

        public void TrackLocation(T loc)
        {
            foreach (var observer in observers)
            {
                if (loc == null)
                    observer.OnError(new Exception());
                else
                    observer.OnNext(loc);
            }
        }

        public void EndTransmission()
        {
            foreach (var observer in observers.ToArray())
                if (observers.Contains(observer))
                    observer.OnCompleted();

            observers.Clear();
        }
    }


    public class actMRActor : actActor
    {
        List<actDataActor<EDDScheme.EDDShipYard>> fShipYardList;
        List<actDataActor<EDDScheme.EDDCommodity>> fCommo;

        IConnectableObservable<actDataActor<EDDScheme.EDDCommodity>> source = null;
        IObserver<actDataActor<EDDScheme.EDDCommodity>> target = null;

        // LocationTracker<actDataActor<EDDScheme.EDDCommodity>> source = new LocationTracker<actDataActor<EDDScheme.EDDCommodity>>();

        IDisposable subscription = null;
        IScheduler scheduler = new EventLoopScheduler();


        public actMRActor()
        {
            fShipYardList = new List<actDataActor<EDDScheme.EDDShipYard>>();
            fCommo = new List<actDataActor<EDDScheme.EDDCommodity>>();

            CancellationToken ct = new CancellationToken();

            Become(new bhvBehavior<Tuple<MRQuery, string>>(AddData));

            source = Observable.Create<actDataActor<EDDScheme.EDDCommodity>>(observer =>
                {
                    var task = Task<IDisposable>.Run(() =>
                        {
                            while (true)
                            {
                                var res = Receive(t => { return t is actDataActor<EDDScheme.EDDCommodity>; }).Result as actDataActor<EDDScheme.EDDCommodity>;
                                observer.OnNext(res);
                            }
                            return Disposable.Empty;
                        });
                    return task;
                }
                ).Publish();

            // asynsub
            source
                .Subscribe(x => 
            {
                // get data from actor
                x.SendMessage(new Tuple<string, IActor>("", this));
                var res = Receive(t => { return t is EDDScheme.EDDCommodity; }).Result as EDDScheme.EDDCommodity;
                Console.WriteLine("Value {0}",res.Message.StationName) ;
            },
                ex => Console.WriteLine("on error {0}",ex.Message),
                () => Console.WriteLine("Completed"));

            subscription = source
            .Select(t => t.Tag)
                .Do(d => Console.WriteLine("Do "))
                .Subscribe(x => 
            {
                Console.WriteLine("On Next {0}",x.Id) ;
            },
                ex => Console.WriteLine("on error {0}",ex.Message),
                () => Console.WriteLine("Completed"));

            source.Connect();
            // Become(new bhvBehavior<Tuple<MRQuery, string, IActor>>(FindData));
        }


        private void FindData(Tuple<MRQuery,string,IActor> msg)
        {
            // Send to all shipyard
            foreach(var s in fShipYardList)
            {
                s.FindData(Tuple.Create(msg.Item2,msg.Item3));
            }
            // send to all commo
            foreach (var c in fCommo)
            {
                c.FindData(Tuple.Create(msg.Item2, msg.Item3));
            }
        }

        private void AddData(Tuple<MRQuery,string> msg)
        {
            // try to parse
            try
            {
                var a = JsonConvert.DeserializeObject<ARnEdSpy.EDDScheme.EDDShipYard>(msg.Item2);
                fShipYardList.Add(new actDataActor<EDDScheme.EDDShipYard>(a));
            }
            catch(Exception e)
            {
                Console.WriteLine(e.Message);
            }
            // try to parse
            actDataActor<EDDScheme.EDDCommodity> act = null;
            try
            {
                var a = JsonConvert.DeserializeObject<ARnEdSpy.EDDScheme.EDDCommodity>(msg.Item2);
                act = new actDataActor<EDDScheme.EDDCommodity>(a);
                fCommo.Add(act);
                SendMessage(act);
                // source.Connect();
                // source.Connect();
                // source.TrackLocation(act);
                // actObservable<actDataActor<EDDScheme.EDDCommodity>>.instance.Value.Track(act);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }

    }

    public class actDataActor<T> : actActor
    {
        T fData;
        public actDataActor(T msg)
        {
            fData = msg;
            Become(new bhvBehavior<Tuple<string,IActor>>(DoFindData));
        }

        private void DoFindData(Tuple<string,IActor> msg)
        {
            msg.Item2.SendMessage(fData);
        }

        public void FindData(Tuple<string,IActor> msg)
        {
            SendMessage(msg);
        }
    }
}
