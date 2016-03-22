using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebAngular.Controllers
{
    public class WebAngularController : Controller
    {
        // GET: WebAngular
        public ActionResult One(string value)
        {
            return View();
        }
    }
}