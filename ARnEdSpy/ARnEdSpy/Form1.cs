using Actor.Base;
using Actor.Server;
using ARnEdSpy.MqListener;
using ARnEdSpy.MRActor;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NJsonSchema;
using NJsonSchema.CodeGeneration;
using System.Windows.Forms;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Reactive;
using System.Reactive.Threading;
using System.Reactive.Concurrency;
using System.Reactive.Disposables;
using ARnEdSpy.RSSActor;
using System.Globalization;

namespace ARnEdSpy
{
    public partial class Form1 : Form
    {
        actMRActor actor;

        public Form1()
        {
            InitializeComponent();
        }

        IActor actListener;
        actStringCatcher catcher;

        public class MyArgs : EventArgs
        {
            public string Data { get; set; }
        }

        public event EventHandler<MyArgs> MyEvent;
        public event EventHandler<MyArgs> RSSEvent;


        public void AttachToEvent(EventHandler<MyArgs> anEvent, string msg)
        {
            // Write some code that does something useful here 
            // then raise the event. You can also raise an event 
            // before you execute a block of code.
            OnRaiseCustomEvent(anEvent, new MyArgs() { Data = msg });

        }

        // Wrap event invocations inside a protected virtual method 
        // to allow derived classes to override the event invocation behavior 
        public virtual void OnRaiseCustomEvent(EventHandler<MyArgs> anEvent, MyArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler<MyArgs> handler = anEvent;

            // Event will be null if there are no subscribers 
            if (handler != null)
            {
                // Format the string to send inside the CustomEventArgs parameter
                e.Data += String.Format(" at {0}", DateTime.Now.ToString());

                // Use the () operator to raise the event.
                handler(this, e);
            }
        }

        IConnectableObservable<EventPattern<MyArgs>> observable;
        IConnectableObservable<EventPattern<MyArgs>> observablequote;
        IDisposable console;
        IDisposable count;
        IDisposable listbox;
        int msgnbr = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            actor = new actMRActor();
            ActorServer.Start("localhost", 80, false);
            catcher = new actStringCatcher();
            catcher.SetEvent(this, new EventHandler<string>(EvHandler));

            observable = Observable.FromEventPattern<MyArgs>(this, "MyEvent").Publish();

            count = observable.Subscribe(x =>
            {
                msgnbr++;
                label1.Text = string.Format("Messages received : {0}", msgnbr);
            });

            console = observable
                .Select(t => t.EventArgs.Data)
                .Do(d => Console.WriteLine("Do "))
                .Subscribe(
                    x =>
                    {
                        Console.WriteLine("On Next {0}", (x + "0123456789").Substring(0, 10));
                        textBox1.Text = x.Trim();
                    },
                    ex => Console.WriteLine("on error {0}", ex.Message),
                    () => Console.WriteLine("Completed")
                );

            listbox = observable
                .Select(t => t.EventArgs.Data)
                .Subscribe(
                    x =>
                    {
                        listBox1.Items.Add(x.Substring(20, 10));
                    },
                    ex => Console.WriteLine("on error {0}", ex.Message),
                    () => Console.WriteLine("Completed")
                );

            observable.Connect();

            actListener = new actZeroMQListener(catcher);
        }

        protected void EvHandler(object sender, string e)
        {
            AttachToEvent(MyEvent, e.Trim());
            actor.SendMessage(Tuple.Create(MRQuery.AddData, e.Trim()));
            catcher.NextMessage();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string s = listBox1.SelectedItem as string ;
            //textBox1.Text = s;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var schema = JsonSchema4.FromJson(textBox1.Text);
            var generator = new NJsonSchema.CodeGeneration.CSharp.CSharpGenerator(schema);
            var file = generator.GenerateFile();
            textBox1.Text = file;
            Clipboard.SetText(file);
        }

        private actStringCatcher sc;

        private void button3_Click(object sender, EventArgs e)
        {
            sc = new actStringCatcher();
            sc.SetEvent(this, new EventHandler<string>(RSSHandler));

            observablequote = Observable.FromEventPattern<MyArgs>(this, "RSSEvent").Publish();
            observablequote
                .Select(t => t.EventArgs.Data)
                .Subscribe(
                    x =>
                    {
                        tbRss.AppendText(x);
                        tbRss.AppendText(Environment.NewLine);
                    },
                    ex => Console.WriteLine("on error {0}", ex.Message),
                    () => Console.WriteLine("Completed")
                );
            observablequote.Connect();

            var timer = Observable.Generate(0L, i => true, i => i + 1, i => i, i => TimeSpan.FromSeconds(12))
                .Publish();
            timer.Subscribe(
            x =>
            {
                new actYahooQuote("EURUSD", sc);
                new actYahooQuote("EURAUD", sc);
                new actYahooQuote("EURGBP", sc);
            });
            timer.Connect();

            labelQuote("USD");
            labelQuote("AUD");
            labelQuote("GBP");

            /* var rss = new actRSSReader(https://fr.finance.yahoo.com/actualites/categorie-devises/?format=rss,sc); 
             */
        }

        private IDisposable labelQuote(string ccy)
        {
            // var obsQuotes = observablequote.Replay(1024);
            var subs = observablequote
                .Select(t => t.EventArgs.Data)
                .Where(t => t.Contains(ccy))
                .Select(t =>
                    {
                        var quotestr = t.Substring(6, t.Length - 6).Substring(0, 6);
                        double quote = double.NaN;
                        double.TryParse(quotestr, NumberStyles.Any, CultureInfo.InvariantCulture, out quote);
                        return quote;
                    })
                .Scan((d1, d2) => d1 < d2 ? d2 : d1)
                .DistinctUntilChanged()
                .Subscribe(
                    x =>
                    {
                        switch(ccy)
                        {
                            case "USD": lblEURUSD.Text = ccy + " " + x.ToString(); break;
                            case "AUD": lblEURAUD.Text = ccy + " " + x.ToString(); break;
                            case "GBP": lblEURGBP.Text = ccy + " " + x.ToString(); break;
                            default: break;
                        }
                    },
                    ex => Console.WriteLine("on error {0}", ex.Message),
                    () => Console.WriteLine("Completed"));
            // obsQuotes.Connect();
            return subs;
        }

        protected void RSSHandler(object sender, string e)
        {
            AttachToEvent(RSSEvent, e);
            sc.NextMessage();
        }

        private void button4_Click(object sender, EventArgs e)
        {
            var actobs = new actObservable<string>();
            
            var obs1 = Observable.Generate(0L, i => i < 100, i => i + 1, i => i, i => TimeSpan.FromMilliseconds(500))
            .Publish();
            obs1.Subscribe(
            x =>
            {
                var action = new ActionActor<string>();
                action.SendAction(() =>
                {
                    string s = x.ToString(CultureInfo.InvariantCulture);
                    actobs.SendMessage(s);
                });
            }) ;
            obs1.Connect();

            var obs2 = Observable.Generate(0L, i => i > -100, i => i - 1, i => i, i => TimeSpan.FromMilliseconds(500))
            .Publish();
            obs2.Subscribe(
            x =>
            {
                var action = new ActionActor<string>();
                action.SendAction(() =>
                {
                    string s = x.ToString(CultureInfo.InvariantCulture);
                    actobs.SendMessage("neg "+s);
                });
            });
            obs2.Connect();


            var result = obs1.Merge(obs2)
                // .Scan((d1, d2) => Math.Abs(d1) < Math.Abs(d2) ? d2 : d1)
                .Subscribe
                (
                x =>
                {
                    this.Invoke((MethodInvoker) delegate
                    {
                            textBox1.AppendText(x + Environment.NewLine);
                    });
                }
                );
            // result.Connect();
        }
    }
}
