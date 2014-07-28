using CL.Interfaces;
using CL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ColComSystem.Controllers
{
    public class HomeController : Controller
    {

        #region 初始化

        private readonly INewsRepository _newsRepository = null;

        public HomeController(INewsRepository newsRepository)
        {
            _newsRepository = newsRepository;
        }

        #endregion


        public ActionResult Index()
        {
            List<News> list=_newsRepository.All().ToList<News>();
            string a = "test";
            ViewBag.Message = "Modify this template to jump-start your ASP.NET MVC application.";

            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your app description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}
