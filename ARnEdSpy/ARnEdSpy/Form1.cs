using Actor.Base;
using Actor.Server;
using ARnEdSpy.MqListener;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ARnEdSpy
{
    public partial class Form1 : Form
    {
        List<String> JSonList;
        int msg = 0;

        public Form1()
        {
            InitializeComponent();
        }

        IActor actListener;
        actStringCatcher catcher;
        private void button1_Click(object sender, EventArgs e)
        {
            JSonList = new List<string>();
            // listBox1.DataSource = JSonList;
            ActorServer.Start("localhost", 80, false);
            catcher = new actStringCatcher();
            catcher.SetEvent(listBox1, new EventHandler<string>(EvHandler));
            actListener = new actZeroMQListener(catcher);

        }

        protected void EvHandler(object sender, string e)
        {
            JSonList.Add(e.Trim());
            msg++;
            label1.Text = string.Format("Messages received : {0}",msg);
            textBox1.Text = e.Trim();
            catcher.NextMessage();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            //string s = listBox1.SelectedItem as string ;
            //textBox1.Text = s;
        }
    }
}
