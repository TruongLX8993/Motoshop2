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
    public class CategoryController : BaseController
    {
        ICategoryRepository categoryRepository = new CategoryRepository();
        // GET: Menu
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListData(int type, int page)
        {
            var model = categoryRepository.GetAll();
            var lst = new List<Category>();
            foreach (var item in model.Where(g => g.ParentId == 0).OrderBy(x => x.OrderDisplay).ToList())
            {
                lst = RenderMenu(item, model.ToList());
            }
            if (type != 0)
                lst = lst.Where(x => x.Type == type).ToList();
            //var totalAdv = lst.Count();
            //lst = lst.Skip((page - 1) * 20).Take(20).ToList();
            TempData["Page"] = page;
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Category/_ListData.cshtml", lst),
                //totalPages = Math.Ceiling(((double)totalAdv / 20)),
            }, JsonRequestBehavior.AllowGet);
        }

        List<Category> lst = new List<Category>();
        private List<Category> RenderMenu(Category parent, List<Category> listfull)
        {
            var lstChilds = listfull.Where(g => g.ParentId == parent.ID);
            lst.Add(new Category
            {
                ID = parent.ID,
                Name = (new String('-', ((int)parent.Level - 1) * 2)) + " " + parent.Name,
                Icon = parent.Icon,
                ParentId = parent.ParentId,
                Level = parent.Level,
                OrderDisplay = parent.OrderDisplay
            });
            foreach (var child in lstChilds)
            {
                RenderMenu(child, listfull);
            }
            return lst;
        }

        public ActionResult Add()
        {
            TempData["Category"] = categoryRepository.GetAll();
            return Json(RenderViewToString("~/Areas/Admin/Views/Category/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Add(Category model)
        {
            try
            {
                var obj = categoryRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == model.Name.Trim());
                if (obj != null)
                {
                    return Json(new { IsSuccess = false, Messenger = "Tên danh mục đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                if (model.ParentId == 0)
                    model.Level = 1;
                else
                {
                    var cateParent = categoryRepository.GetAll().FirstOrDefault(x => x.ID == model.ParentId);
                    model.Level = cateParent.Level + 1;
                }
                categoryRepository.Add(model);
                return Json(new { IsSuccess = true, Messenger = "Thêm mới thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Messenger = "Thêm mới thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public ActionResult Edit(int id)
        {
            var obj = categoryRepository.Find(id);
            TempData["Category"] = categoryRepository.GetAll();
            return Json(RenderViewToString("~/Areas/Admin/Views/Category/_Edit.cshtml", obj), JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Category model)
        {
            try
            {
                var obj = categoryRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == model.Name.Trim() && x.ID != model.ID);
                if (obj != null)
                {
                    return Json(new { IsSuccess = false, Messenger = "Tên danh mục đã tồn tại" }, JsonRequestBehavior.AllowGet);
                }
                if (model.ParentId == 0)
                    model.Level = 1;
                else
                {
                    var cateParent = categoryRepository.GetAll().FirstOrDefault(x => x.ID == model.ParentId);
                    model.Level = cateParent.Level + 1;
                }
                categoryRepository.Edit(model);
                return Json(new { IsSuccess = true, Messenger = "Cập nhật thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Messenger = "Cập nhật thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                categoryRepository.Delete(id);
                return Json(new { IsSuccess = true, Messenger = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Messenger = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
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
                    categoryRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} danh mục", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
