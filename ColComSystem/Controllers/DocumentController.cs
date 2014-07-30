using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColComSystem.Controllers
{
    public class DocumentController : Controller
    {
        //
        // GET: /Document/

        public ActionResult Documents()
        {
            return View();
        }

        public ActionResult DocumentDetails()
        {
            return View();
        }

    }
}
