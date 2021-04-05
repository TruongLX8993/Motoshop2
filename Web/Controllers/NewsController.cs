
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class NewsController : BaseController
    {
        INewsRepository newsRepository = new NewsRepository();
        // GET: News
        public ActionResult Index(int? page)
        {
            int pageSite = 6;
            int pageNumber = (page ?? 1);
            var model = newsRepository.GetAll().OrderByDescending(x => x.CreatedDate).ToPagedList(pageNumber, pageSite);
            return View(model);
        }
        public ActionResult Detail(string linkseo)
        {
            var news =  newsRepository.GetAll().FirstOrDefault(x=>x.LinkSeo.Equals(linkseo));
            return View(news);
        }
    }
}