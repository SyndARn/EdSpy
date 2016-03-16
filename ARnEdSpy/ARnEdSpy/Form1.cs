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


        public void AttachToEvent(string msg)
        {
            // Write some code that does something useful here 
            // then raise the event. You can also raise an event 
            // before you execute a block of code.
            OnRaiseCustomEvent(new MyArgs() { Data = msg });

        }

        // Wrap event invocations inside a protected virtual method 
        // to allow derived classes to override the event invocation behavior 
        public virtual void OnRaiseCustomEvent(MyArgs e)
        {
            // Make a temporary copy of the event to avoid possibility of 
            // a race condition if the last subscriber unsubscribes 
            // immediately after the null check and before the event is raised.
            EventHandler<MyArgs> handler = MyEvent;

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
        IDisposable console;
        IDisposable count;
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
                            Console.WriteLine("On Next {0}", (x+"0123456789").Substring(0,10));
                            textBox1.Text = x.Trim();
                        },
                    ex => Console.WriteLine("on error {0}", ex.Message),
                    () => Console.WriteLine("Completed")
                );

            observable.Connect();

            actListener = new actZeroMQListener(catcher);
        }

        protected void EvHandler(object sender, string e)
        {
            AttachToEvent(e.Trim());
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
    }
}
