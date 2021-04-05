using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class CategoryController : BaseController
    {
        private ICategoryRepository categoryRepository = new CategoryRepository();
        private IProductRepository productRepository = new ProductRepository();
        // GET: Category
        public ActionResult Index(string linkseo,int? page)
        {
            int pagesite = 12;
            int pagenumber = (page ?? 1);
            var model = productRepository.GetAll().OrderByDescending(x => x.CreatedDate).ToPagedList(pagenumber, pagesite);
            return View(model);
        }
        public PartialViewResult GetCategories()
        {
            var lstCate = categoryRepository.GetAll().OrderBy(x => x.OrderDisplay).ToList();
            return PartialView(lstCate);
        }
    }
}