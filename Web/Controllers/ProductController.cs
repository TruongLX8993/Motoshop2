using System;
using System.Linq;
using System.Web;
using PagedList;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Repository.Entity;
using Web.Repository;
using Web.Model;
using Web.Core;

namespace Web.Controllers
{
    public class ProductController : BaseController
    {
        private IProductRepository productRepository = new ProductRepository();
        private ICategoryRepository categoryRepository = new CategoryRepository();
        private string pathname = "";
        // GET: Product
        public ActionResult Index(string linkseo,int?pageIndex)
        {
            switch (linkseo)
            {
                case "tuyen-dung":
                    return RedirectToAction("Index", "Recruitment");
                case "tin-tuc":
                    return RedirectToAction("Index", "News");
                case "lien-he":
                    return RedirectToAction("Index", "About");
                default:
                    break;
            }

            int pagesite = 12;
            int pagenumber = (pageIndex ?? 1);
            var cate = categoryRepository.GetAll().FirstOrDefault(x => x.LinkSeo.Equals(linkseo));
            int cateId = 0;
            string cateName = "";
            string path = "";
            if (cate != null)
            {
                if(cate.Level > 1)
                {
                    path = GetPath(cate) + cate.Name;
                }
                else
                {
                    path = cate.Name;
                }
                cateName = cate.Name;
                cateId = cate.ID;
            }
            TempData["CateName"] = cateName;
            TempData["MapPath"] = path;
            var model = productRepository.GetAll().Where(x=>x.CategoryId == cateId).OrderByDescending(x => x.CreatedDate).ToPagedList(pagenumber, pagesite);
            return View(model);
        }
         
        public ActionResult Search(int? pageIndex, string title)
        {
            int pagesite = 12;
            int pagenumber = (pageIndex ?? 1);
            
            var model = productRepository.GetAll();
            if (!string.IsNullOrEmpty(title))
            {
                model = model.Where(x => HelperString.UnsignCharacter(x.Name.ToLower().Trim()).Contains(HelperString.UnsignCharacter(title.ToLower().Trim())));
            }
            model = model.OrderByDescending(x => x.CreatedDate).ToPagedList(pagenumber, pagesite);
            return View(model);
        }
        public ActionResult Detail(string linkseo)
        {
            var obj = productRepository.GetAll().FirstOrDefault(x => x.LinkSeo.Equals(linkseo));
            var cate = categoryRepository.GetAll().FirstOrDefault(x => x.ID == obj.CategoryId);
            string path = "";
            if (cate != null)
            {
                if (cate.Level > 1)
                {
                    path = GetPath(cate) + cate.Name;
                }
                else
                {
                    path = cate.Name + "/";
                }
            }
           
            TempData["MapPath"] = path;
            var lstProduct = productRepository.GetAll().Where(x => x.CategoryId == obj.CategoryId && x.ID != obj.ID).ToList();
            ViewBag.RelatedProduct = lstProduct;
            return View(obj);
        }
       
        private string GetPath(Category category)
        {
            var cate = categoryRepository.GetAll().FirstOrDefault(x => x.ID == category.ParentId);
            int level = 0;
            if (cate != null)
            {
                pathname += cate.Name + "/";
                level = (int)category.Level;
                for (int i = level; i >= 1; i--)
                {
                    GetPath(cate);
                }
            }
            return pathname;
        }
    }
}