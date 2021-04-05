using System;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class InfoUsefulController : BaseController
    {
        private IInfoUsefulRepository infoUsefulRepository = new InfoUsefulRepository();
        // GET: News
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListData(int page)
        {
            var model = infoUsefulRepository.GetAll();
            var totalAdv = model.Count();
            model = model.Skip((page - 1) * 10).Take(10).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/InfoUseful/_ListData.cshtml", model),
                totalPages = Math.Ceiling(((double)totalAdv / 10)),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(InfoUseful model,string close)
        {
            try
            {
                var obj = infoUsefulRepository.GetAll().FirstOrDefault(x=>x.MetaTitle==model.MetaTitle);
                if (obj != null)
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        Message = "Tên tin tức đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.Contents))
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Message = "Vui lòng thêm nội dung bài viết",
                    }, JsonRequestBehavior.AllowGet);
                }
                infoUsefulRepository.Add(model);
                return Json(new
                {
                    Close = close,
                    IsSuccess = true,
                    Message = "Thêm mới thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Thêm mới thất bại"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = infoUsefulRepository.Find(id);
            return View(obj);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(InfoUseful model)
        {
            try
            {
                var obj = infoUsefulRepository.GetAll().FirstOrDefault(x => x.MetaTitle == model.MetaTitle && x.ID != model.ID);
                if (obj != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Message = "Tin tức đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                if (string.IsNullOrEmpty(model.Contents))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        Message = "Vui lòng thêm nội dung bài viết",
                    }, JsonRequestBehavior.AllowGet);
                }
                var page = Session["PAGE"];
                if (page == null)
                    page = 1;
                infoUsefulRepository.Edit(model);
                return Json(new {
                    Page = (int)page,
                    IsSuccess = true,
                    Message = "Cập nhật thành công",
                    JsonRequestBehavior.AllowGet });
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Cập nhật thất bại", JsonRequestBehavior.AllowGet });
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                infoUsefulRepository.Delete(id);
                return Json(new { IsSuccess = true, Message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}