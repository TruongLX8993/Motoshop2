using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using PagedList;
using Web.BaseSecurity;
using Web.Repository;
using Web.Repository.Entity;
using Web.Core;

namespace Web.Controllers
{
    public class GalleryController : BaseController
    {
        
        readonly IGalleryRepository _galleryRepository = new GalleryRepository();
        readonly IGalleryCategoryRepository _galleryCategoryRepository = new GalleryCategoryRepository();
       
        public ActionResult Index(int catid = 0)
        {
            var lang = GetLangCode();
            int page = !string.IsNullOrEmpty(Request["page"]) ? Convert.ToInt32(Request["page"]) : 1;

            if (catid == 0)
            {
                var objGalleryCategory = _galleryCategoryRepository.GetAll().Where(g => g.Active == true).OrderBy(g => g.Ordering).FirstOrDefault();
                catid = objGalleryCategory.ID;
                //ViewBag.title = HelperString.RemoveMark(objGalleryCategory.Name);
                ViewBag.title = objGalleryCategory.Name;
            }
            else
            {
                var objGalleryCategory = _galleryCategoryRepository.Find(catid);
                //ViewBag.title = HelperString.RemoveMark(objGalleryCategory.Name);
                ViewBag.title = objGalleryCategory.Name;
            }
            var lstGallery = _galleryRepository.GetAll().Where(g => g.CategoryId == catid && g.Status == true).OrderBy(g=>g.Ordering).ToList();
            ViewBag.lstGallery = lstGallery;
            var lstGalleryCategory = _galleryCategoryRepository.GetAll().Where(g => g.ID != catid && g.Active == true).OrderBy(g => g.Ordering);
            return View(lstGalleryCategory.ToPagedList(page, Webconfig.RowLimit));
        }
       
    }
}
