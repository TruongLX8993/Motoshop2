using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class HeaderController : Controller
    {
        private readonly IHeaderRepository headerRepository = new HeaderRepository();
        // GET: Footer
        public ActionResult Header()
        {
            var header = headerRepository.GetAll().FirstOrDefault();
            return PartialView(header);
        }
    }
}