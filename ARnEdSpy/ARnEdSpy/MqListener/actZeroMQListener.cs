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
    public class actZeroMQListener : actActor
    {
        ZSocket listener;
        ZContext ztx;
        actActor fAnswerActor;

        public actZeroMQListener(actActor answerActor, string anUrl = @"tcp://eddn-relay.elite-markets.net:9500")
            : base()
        {
            fAnswerActor = answerActor;
            Become(new bhvBehavior<string>(DoRedirect));
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

    public class actZeroMqReceiver : actActor
    {

        public actZeroMqReceiver(ZSocket subscriber, actActor answer)
        {
            Become(new bhvBehavior<Tuple<ZSocket, actActor>>(DoRedirect));
            SendMessage(Tuple.Create(subscriber, answer));
        }

        private void DoRedirect(Tuple<ZSocket, actActor> msg)
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
