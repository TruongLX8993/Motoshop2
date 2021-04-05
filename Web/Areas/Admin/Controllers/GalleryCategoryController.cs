using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class GalleryCategoryController : BaseController
    {
        //
        // GET: /Admin/GalleryCategory/
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IGalleryCategoryRepository _galleryCategoryReporitory = new GalleryCategoryRepository();
        readonly IGalleryRepository _galleryRep = new GalleryRepository();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string Name, string LangCode, int page = 1)
        {
            var lstgallerycategory = _galleryCategoryReporitory.GetAll().OrderByDescending(g=>g.ID).ToList();
            if (!string.IsNullOrEmpty(Name))
            {
                lstgallerycategory =
                    lstgallerycategory.Where(
                        g => HelperString.UnsignCharacter(g.Name.ToLower().Trim()).Contains(Name.ToLower().Trim())).ToList();
            }
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/GalleryCategory/_ListData.cshtml", lstgallerycategory),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            return Json(RenderViewToString("~/Areas/Admin/Views/GalleryCategory/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_GalleryCategory obj)
        {
            try
            {
                obj.CreatedDate = DateTime.Now;
                _galleryCategoryReporitory.Add(obj);
               
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới danh mục thư viện ảnh thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới danh mục thư viện ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var lstLang = _languagesRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            var objGalleryCategory = _galleryCategoryReporitory.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/GalleryCategory/_Edit.cshtml", objGalleryCategory), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_GalleryCategory obj)
        {
            try
            {
                _galleryCategoryReporitory.Edit(obj);
               
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật danh mục thư viện ảnh thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật danh mục thư viện ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id)
        {
            var obj = _galleryCategoryReporitory.Find(id);
            obj.Active = !obj.Active;
            _galleryCategoryReporitory.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                
                var obj = _galleryCategoryReporitory.Find(id);
                var objGallery = _galleryRep.GetAll().ToList();
                foreach (var item in objGallery)
                {
                    if (item.CategoryId == id)
                    {
                        _galleryRep.Delete(item.ID);
                    }
                }
                _galleryCategoryReporitory.Delete(id);
               
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa danh mục thư viện ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa danh mục thư viện ảnh thành công",
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult DeleteAll(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    var objGallery = _galleryRep.GetAll().ToList();
                    foreach (var item2 in objGallery)
                    {
                        if (item2.CategoryId ==Convert.ToInt32(item))
                        {
                            _galleryRep.Delete(item2.ID);
                        }
                    }
                    _galleryCategoryReporitory.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} danh mục thư viện ảnh", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
