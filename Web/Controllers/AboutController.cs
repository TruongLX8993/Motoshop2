using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class AboutController : Controller
    {
        private readonly IAboutRepository aboutRepository = new AboutRepository();
        // GET: About
        public ActionResult Index()
        {
            var about = aboutRepository.GetAll().FirstOrDefault();
            return View(about);
        }
    }
}