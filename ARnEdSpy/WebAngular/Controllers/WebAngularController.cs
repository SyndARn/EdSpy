using Actor.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

namespace WebAngular.Controllers
{
    public class WebAngularController : Controller
    {
        private static Actor.Server.ActorServer server = null;
        private static Dictionary<string,actCatcher> dico;
        // GET: WebAngular
        public ActionResult Usd(string quote)
        {
            if (! string.IsNullOrEmpty(quote))
            {
                ViewBag.Currency = quote;
                if (server == null)
                {
                    Actor.Server.ActorServer.Start("localhost", 8181, false);
                    server = Actor.Server.ActorServer.GetInstance();
                }
                if (dico == null)
                    dico = new Dictionary<string, actCatcher>();
                actCatcher cat = null;
                if (!dico.TryGetValue(quote, out cat))
                {
                    cat = new actCatcher();
                    dico[quote] = cat;
                }
                var yahoo = new actYahooQuote("EUR" + quote, cat);

                ViewBag.Quote = cat.GetValue();
            }
            return View();
        }

        // GET: WebAngular
        public ContentResult GetQuote(string quote)
        {
            var cr = new ContentResult();
            if (!string.IsNullOrEmpty(quote))
            {
                ViewBag.Currency = quote;
                if (server == null)
                {
                    Actor.Server.ActorServer.Start("localhost", 8181, false);
                    server = Actor.Server.ActorServer.GetInstance();
                }
                if (dico == null)
                    dico = new Dictionary<string, actCatcher>();
                actCatcher cat = null;
                if (!dico.TryGetValue(quote, out cat))
                {
                    cat = new actCatcher();
                    dico[quote] = cat;
                }
                var yahoo = new actYahooQuote("EUR" + quote, cat);

                ViewBag.Quote = cat.GetValue();
                cr.Content = cat.GetValue();
            }
            return cr;
        }
    }

    public class actCatcher : BaseActor
    {
        string fQuote;
        object flock = new object();

        public actCatcher()
        {
            Become(new Behavior<string>(ReceiveString));
        }

        public string GetValue()
        {
            lock(flock)
                return fQuote;
        }

        private void ReceiveString(string msg)
        {
            lock (flock)
                fQuote = msg;
        }
    }

    public class actYahooQuote : BaseActor
    {
        const string model = @"http://download.finance.yahoo.com/d/quotes.csv?e=.csv&f=c4l1&s={0}=X";
        public actYahooQuote(string currencyPair, IActor target)
        {
            Become(new Behavior<Tuple<string, IActor>>(DoQuote));
            SendMessage(Tuple.Create(string.Format(model, currencyPair), target));
        }

        private void DoQuote(Tuple<string, IActor> msg)
        {
            using (var client = new HttpClient())
            {
                using (var hc = new StringContent(msg.Item1))
                {
                    Uri uri = new Uri(msg.Item1);
                    var post = client.PostAsync(uri, hc).Result;
                    string result = post.Content.ReadAsStringAsync().Result;
                    msg.Item2.SendMessage(result);
                }
            }
        }
    }
}