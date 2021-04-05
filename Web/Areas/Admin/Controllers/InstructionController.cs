using System;
using System.Linq;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class InstructionController : BaseController
    {
        readonly IInstructionRepository _instructionRepository = new InstructionRepository();
        public ActionResult Index()
        {
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListData(string keyWord,int pageIndex, int pageSize)
        {
            var lstObj = _instructionRepository.GetAll().ToList();
            if (!string.IsNullOrEmpty(keyWord))
                lstObj = lstObj.Where(x => HelperString.UnsignCharacter(x.Title.ToLower().Trim()).Contains(HelperString.UnsignCharacter(keyWord.ToLower().Trim()))).ToList();
            var totalRow = lstObj.Count();
            TempData["Page"] = pageIndex;
            lstObj = lstObj.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Instruction/_ListData.cshtml", lstObj),
                totalPages = Math.Ceiling(((double)totalRow / pageSize))
            }, JsonRequestBehavior.AllowGet);
        }
        [Authorize(Roles = "Add")]
        public ActionResult Add()
        {
            var lstLang = _instructionRepository.GetAll().ToList();
            TempData["Languages"] = lstLang;
            return View();
        }

        [Authorize(Roles = "Add")]
        [ValidateInput(false)]
        [HttpPost]
        public ActionResult Add(Instruction model)
        {
            try
            {
                var obj = _instructionRepository.GetAll().FirstOrDefault(x => x.Title.Trim().Equals(model.Title.Trim()));
                if (obj != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Tiêu đề đã tồn tại")
                    }, JsonRequestBehavior.AllowGet);
                }
                model.LinkSeo = HelperString.RenderLinkSeo(model.Title);
                model.CreatedDate = DateTime.Now;
                if (_instructionRepository.Add(model))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = "Thêm mới thành công",
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Thêm mới thất bại")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Thêm mới thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Edit(int id)
        {
            var obj = _instructionRepository.Find(id);
            return View(obj);
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(Instruction model)
        {
            try
            {
                var objInst = _instructionRepository.GetAll().FirstOrDefault(x => x.Title.Trim().Equals(model.Title.Trim()) && x.ID != model.ID);
                if (objInst != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Tiêu đề đã tồn tại")
                    }, JsonRequestBehavior.AllowGet);
                }
                model.LinkSeo = HelperString.RenderLinkSeo(model.Title);
                if (_instructionRepository.Edit(model))
                {
                    return Json(new
                    {
                        IsSuccess = true,
                        Messenger = "Cập nhật thành công",
                    }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = string.Format("Cập nhật thất bại")
                    }, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Cập nhật thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        [Authorize(Roles = "Edit")]
        public ActionResult ChangeStatus(int id)
        {
            var obj = _instructionRepository.Find(id);
            _instructionRepository.Edit(obj);
            return Json(new
            {
                IsSuccess = true,
                Messenger = "Thay đổi trạng thái thành công",
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult Delete(int id)
        {
            try
            {
                _instructionRepository.Delete(id);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Xóa thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = string.Format("Xóa thất bại")
                }, JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult DeleteAll(string lstid)
        {
            var arrid = lstid.Split(',');
            var count = 0;
            foreach (var item in arrid)
            {
                try
                {
                    _instructionRepository.Delete(Convert.ToInt32(item));
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
