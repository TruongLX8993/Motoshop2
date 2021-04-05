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
    public class NewsController : BaseController
    {

        INewsRepository newsRepository = new NewsRepository();
        ICategoryRepository categoryRepository = new CategoryRepository();
        // GET: News
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListData(string keyWord, int pageIndex, int pageSize)
        {
            Session["PAGE"] = pageIndex;
            int page = Convert.ToInt32(Request.QueryString["page"]);
            if (page > 1)
                pageIndex = page;
            
            var model = newsRepository.GetAll().ToList();
            if (!string.IsNullOrEmpty(keyWord))
                model = model.Where(x => HelperString.UnsignCharacter(x.MetaTitle.ToLower().Trim()).Contains(HelperString.UnsignCharacter(keyWord.ToLower().Trim()))).ToList();
            var totalAdv = model.Count();
            TempData["Page"] = pageIndex;
            model = model.Skip((pageIndex - 1) * 10).Take(10).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/News/ListData.cshtml", model),
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
            TempData["Category"] = categoryRepository.GetAll().Where(x=>x.Type==1).ToList();
            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(News model,string close)
        {
            try
            {
                var news = newsRepository.GetAll().FirstOrDefault(x=>x.MetaTitle.Trim().Equals(model.MetaTitle.Trim()));
                if (news != null)
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        Message = "Tiêu đề tin tức đã tồn tại",
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
                model.LinkSeo = HelperString.RenderLinkSeo(model.MetaTitle);
                model.CreatedBy =  User.ID;
                model.CreatedDate = DateTime.Now;
                newsRepository.Add(model);
                return Json(new
                {
                    Close = close,
                    IsSuccess = true,
                    Message = "Thêm mới thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Message = "Thêm mới thất bại "
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            TempData["Category"] = categoryRepository.GetAll().Where(x => x.Type == 1).ToList();
            var obj = newsRepository.Find(id);
            return View(obj);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(News model)
        {
            try
            {
                var news = newsRepository.GetAll().FirstOrDefault(x => x.MetaTitle.Trim().Equals(model.MetaTitle.Trim()));
                if (news != null && news.ID != model.ID)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Message = "Tiêu đề tin tức đã tồn tại",
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
                model.LinkSeo = HelperString.RenderLinkSeo(model.MetaTitle);
                newsRepository.Edit(model);
                return Json(new {
                    Page = (int)page,
                    IsSuccess = true,
                    Message = "Cập nhật thành công",
                    JsonRequestBehavior.AllowGet });
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Message = "Cập nhật thất bại", JsonRequestBehavior.AllowGet });
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                newsRepository.Delete(id);
                return Json(new { IsSuccess = true, Message = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult UnPublish(int id)
        {
            try
            {
                newsRepository.UnPublish(id);
                return Json(new { IsSuccess = true, Message = "Hủy đăng bài thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Hủy đăng bài thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Publish(int id)
        {
            try
            {
                newsRepository.Publish(id);
                return Json(new { IsSuccess = true, Message = "Đăng bài thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Message = "Đăng bài thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}