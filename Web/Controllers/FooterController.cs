using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class FooterController : Controller
    {
        private readonly IInfoUsefulRepository infoUsefulRepository = new InfoUsefulRepository();
        private readonly IFooterRepository footerRepository = new FooterRepository();
        // GET: Footer
        public ActionResult LoadFooter()
        {
            var footer = footerRepository.GetAll().FirstOrDefault();
            ViewBag.Footer = footer.Contents;
            var lstinfo = infoUsefulRepository.GetAll().ToList();
            return PartialView(lstinfo);
        }
    }
}