using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actor;
using Actor.Base;
using ZeroMQ;
using System.IO.Compression;
using System.IO;
using Actor.Server;
using Ionic.Zlib;

namespace ARnEdSpy.MqListener
{
    public class actZeroMQListener : BaseActor
    {
        ZSocket listener;
        ZContext ztx;
        BaseActor fAnswerActor;

        public actZeroMQListener(BaseActor answerActor, string anUrl = @"tcp://eddn-relay.elite-markets.net:9500")
            : base()
        {
            fAnswerActor = answerActor;
            Become(new Behavior<string>(DoRedirect));
            SendMessage(anUrl);
        }

        private void DoRedirect(string anUrl)
        {
            ztx = new ZContext();
            listener = new ZSocket(ztx, ZSocketType.SUB);
            listener.Subscribe("");
            listener.ReceiveTimeout = TimeSpan.MaxValue;
            listener.Connect(anUrl);
            new actZeroMqReceiver(listener, fAnswerActor);
        }
    }

    public class actZeroMqReceiver : BaseActor
    {

        public actZeroMqReceiver(ZSocket subscriber, BaseActor answer)
        {
            Become(new Behavior<Tuple<ZSocket, BaseActor>>(DoRedirect));
            SendMessage(Tuple.Create(subscriber, answer));
        }

        private void DoRedirect(Tuple<ZSocket, BaseActor> msg)
        {
            var frame = msg.Item1.ReceiveFrame();
            if (frame != null)
            {
                using (Stream decompress = new ZlibStream(frame, Ionic.Zlib.CompressionMode.Decompress))
                {
                    using (var sr = new StreamReader(decompress))
                    {
                        var json = sr.ReadToEnd();
                        msg.Item2.SendMessage(json);
                    }
                }
            }
            SendMessage(msg);
        }

    }
}
