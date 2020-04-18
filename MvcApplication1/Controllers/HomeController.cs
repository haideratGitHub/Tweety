using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult index()
        {
            return View();

        }
        public ActionResult Index2()
        {
            return View();
        }

    }
}
