using OfficeOpenXml;
using OfficeOpenXml.Style;
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
    public class GalleryController : BaseController
    {
        readonly IGalleryRepository _GalleryRepository = new GalleryRepository();
        readonly IGalleryCategoryRepository _galleryCategoryRepository = new GalleryCategoryRepository();
        readonly ILanguagesRepository _languagesRepository = new LanguagesRepository();
        readonly IUserAdminRepository _userAdminRepository = new UserAdminRepository();
        //
        // GET: /Admin/Gallery/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            var objUser = _userAdminRepository.Find(User.ID);
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            var lstGalleryCategory = _galleryCategoryRepository.GetAll();
            TempData["GalleryCategory"] = lstGalleryCategory.ToList();
            return View();
        }

        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string LangCode, int PageElementId = 0, int page = 1,string NgayDang=null,string NgayKet=null)
        {
            var objUser = _userAdminRepository.Find(User.ID);
            var lstLanguages = _languagesRepository.GetAll();
            TempData["Languages"] = lstLanguages.ToList();
            var lstGallery = _GalleryRepository.GetAll();
            if (!string.IsNullOrEmpty(NgayDang)&& !string.IsNullOrEmpty(NgayKet))
            {
                var ngaydang = HelperDateTime.ConvertDate(NgayDang);
                var ngayket = HelperDateTime.ConvertDate(NgayKet);
                lstGallery = lstGallery.Where(s=>s.CreatedDate.Date>= ngaydang && s.CreatedDate.Date<= ngayket);
            }
            var totalGallery = lstGallery.Count();
            lstGallery = lstGallery.OrderByDescending(g => g.ID).Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit);
            TempData["LstDatage"] = lstGallery.ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Gallery/_ListData.cshtml", lstGallery),
                totalPages = Math.Ceiling(((double)totalGallery / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var objUser = _userAdminRepository.Find(User.ID);
            var lstGalleryCategory = _galleryCategoryRepository.GetAll();
            TempData["GalleryCategory"] = lstGalleryCategory.ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/Gallery/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }
       
        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(tbl_Gallery obj)
        {
            obj.CreatedDate = DateTime.Now;
            obj.CreatedBy = User.ID;

            try
            {
                _GalleryRepository.Add(obj);
               
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới ảnh slide thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới ảnh slide thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            var objUser = _userAdminRepository.Find(User.ID);
            var objGallery = _GalleryRepository.Find(id);
            var lstGalleryCategory = _galleryCategoryRepository.GetAll();
            TempData["GalleryCategory"] = lstGalleryCategory.ToList();

            /*var lstpageElements = _pageElementRepository.GetAll().Where(g => arrPageElement.Contains(g.ID) && g.LangCode == objGallery.LangCode);
            TempData["PageElement"] = lstpageElements.ToList();*/
            return Json(RenderViewToString("~/Areas/Admin/Views/Gallery/_Edit.cshtml", objGallery), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(tbl_Gallery obj)
        {
            obj.ModifiedBy = User.ID;
            obj.ModifiedDate = DateTime.Now;
            try
            {
                _GalleryRepository.Edit(obj);
               
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật slide ảnh thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật slide ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _GalleryRepository.Find(id);
                _GalleryRepository.Delete(id);
               
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa slide ảnh thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Xóa slide ảnh thành công",
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
                    _GalleryRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} slide ảnh", count),
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id)
        {
            var obj = _GalleryRepository.Find(id);
            obj.Status = !obj.Status;
            _GalleryRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult UpdatePosition(string value)
        {
            var arrValue = value.Split('|');
            foreach (var item in arrValue)
            {
                var id = item.Split(':')[0];
                var pos = item.Split(':')[1];
                var obj = _GalleryRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _GalleryRepository.Edit(obj);

                }
                catch (Exception)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Cập nhật vị trí thất bại")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Cập nhật vị trí thành công",
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
