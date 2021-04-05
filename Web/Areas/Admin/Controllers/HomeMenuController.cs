using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class HomeMenuController : BaseController
    {
        readonly IHomeMenuRepository _homeMenuRepository = new HomeMenuRepository();
        //
        // GET: /Admin/HomeMenu/
        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        [HttpPost]
        public ActionResult ListData(string keyWord,int page = 1)
        {
            var model = _homeMenuRepository.GetAll();
            var lst = new List<HomeMenu>();
            foreach (var item in model.Where(g => g.ParentId == 0).OrderBy(x => x.Ordering).ToList())
            {
                lst = RenderMenu(item, model.ToList());
            }
           
            if (!string.IsNullOrEmpty(keyWord))
                lst = lst.Where(x => HelperString.UnsignCharacter(x.Name.ToLower().Trim()).Contains(HelperString.UnsignCharacter(keyWord.ToLower().Trim()))).ToList();
            
            var totalAdv = lst.Count();
            lst = lst.Skip((page - 1) * 20).Take(20).ToList();
            TempData["Page"] = page;
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/HomeMenu/_ListData.cshtml", lst),
                totalPages = Math.Ceiling(((double)totalAdv / 20)),
            }, JsonRequestBehavior.AllowGet);
        }
        List<HomeMenu> lst = new List<HomeMenu>();
        private List<HomeMenu> RenderMenu(HomeMenu parent, List<HomeMenu> listfull)
        {
            var lstChilds = listfull.Where(g => g.ParentId == parent.ID);
            lst.Add(new HomeMenu
            {
                ID = parent.ID,
                Name = (new String('-', ((int)parent.Level - 1) * 2))+" "+parent.Name,
                Icon = parent.Icon,
                ParentId = parent.ParentId,
                Level = parent.Level,
                Ordering = parent.Ordering,
                CreatedDate = parent.CreatedDate
            });
            foreach (var child in lstChilds)
            {
                RenderMenu(child, listfull);
            }
            return lst;
        }

        [Authorize(Roles = "Index")]
        public ActionResult GetAllByLangCode(string LangCode)
        {
            LangCode = LangCode ?? Webconfig.LangCodeVn;
            var lstHomeMenu = _homeMenuRepository.GetAll().ToList();
            var lstLevel = Common.CreateLevel(lstHomeMenu.ToList());
            return Json(lstLevel, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            TempData["MenuHome"] = _homeMenuRepository.GetAll().ToList();
            return Json(RenderViewToString("~/Areas/Admin/Views/HomeMenu/_Create.cshtml"), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Add")]
        [HttpPost]
        public ActionResult Add(HomeMenu obj)
        {
            try
            {
                var menu = _homeMenuRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == obj.Name.Trim());
                if(menu != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên menu đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                obj.LinkSeo = HelperString.RenderLinkSeo(obj.Name);
                obj.CreatedDate = DateTime.Now;
                _homeMenuRepository.Add(obj);
                
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Thêm mới menu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới menu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Edit(int id)
        {
            TempData["MenuHome"] = _homeMenuRepository.GetAll().ToList();
            var objHomeMenu = _homeMenuRepository.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/HomeMenu/_Edit.cshtml", objHomeMenu), JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        [HttpPost]
        public ActionResult Edit(HomeMenu obj)
        {
            try
            {
                var munuName = _homeMenuRepository.GetAll().FirstOrDefault(x => x.Name.Trim() == obj.Name.Trim() && x.ID != obj.ID);
                if(munuName!= null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên menu đã tồn tại",
                    }, JsonRequestBehavior.AllowGet);
                }
                obj.LinkSeo = HelperString.RenderLinkSeo(obj.Name);
                _homeMenuRepository.Edit(obj);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật menu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật menu thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Delete")]
        [HttpPost]
        public ActionResult Delete(int id)
        {
            try
            {
                _homeMenuRepository.Delete(id);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Xóa menu thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa menu thất bại")
                }, JsonRequestBehavior.AllowGet);
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
                    // xoa danhh muc
                    _homeMenuRepository.Delete(Convert.ToInt32(item));
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} menu", count),
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
                var obj = _homeMenuRepository.Find(Convert.ToInt32(id));
                obj.Ordering = Convert.ToInt32(pos);
                try
                {
                    _homeMenuRepository.Edit(obj);

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
