using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColComSystem.Controllers
{
    public class ProductController : Controller
    {
        //
        // GET: /Product/

        public ActionResult Products()
        {
            return View();
        }

        public ActionResult ProductDetails()
        {
            return View();
        }


    }
}
