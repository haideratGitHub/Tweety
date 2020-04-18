using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Controllers
{
    public class view1Controller : Controller
    {
        //
        // GET: /view1/

        public ActionResult index()
        {
            return View();
        }

    }
}
