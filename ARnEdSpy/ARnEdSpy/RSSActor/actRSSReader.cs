using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Actor;
using Actor.Base;
using System.Xml;
using System.ServiceModel.Syndication;
using System.Net;
using System.Net.Http;


namespace ARnEdSpy.RSSActor
{
    public class actRSSReader : actActor
    {
        string fUrl;
        public actRSSReader(string anUrl, IActor target)
        {
            fUrl = anUrl;
            Become(new bhvBehavior<Tuple<string,IActor>>(DoSyndication)) ;
            SendMessage(Tuple.Create(anUrl,target)) ;
        }

        private void DoSyndication(Tuple<string,IActor> msg)
        {
            XmlReader reader = XmlReader.Create(msg.Item1);
            SyndicationFeed feed = SyndicationFeed.Load(reader);
            reader.Close();
            foreach(SyndicationItem item in feed.Items)
            {
                msg.Item2.SendMessage(item.Title.Text);
            }
        }
    }

    public class actYahooQuote : actActor 
    { 
        const string model = @"http://download.finance.yahoo.com/d/quotes.csv?e=.csv&f=c4l1&s={0}=X" ;
        public actYahooQuote(string currencyPair, IActor target)
        {
            Become(new bhvBehavior<Tuple<string, IActor>>(DoQuote));
            SendMessage(Tuple.Create(string.Format(model,currencyPair), target));
        }

        private void DoQuote(Tuple<string, IActor> msg )
        {
            using (var client = new HttpClient())
            {
                using (var hc = new StringContent(msg.Item1))
                {
                    Uri uri = new Uri(msg.Item1);
                    var post = client.PostAsync(uri, hc).Result ;
                    string result = post.Content.ReadAsStringAsync().Result;
                    msg.Item2.SendMessage(result);
                }
            }
        }
    }
}
