using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Linq.Expressions;
using System.Web;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class ContactController : BaseController
    {
        readonly IInstructionReporitory _ContactReporitory = new ContactReporitory();

        [Authorize(Roles = "Index")]
        public ActionResult Index()
        {            
            return View();
        }
        [Authorize(Roles = "Index")]
        public ActionResult ListDataAdmin(string Status, int page = 1)
        {
            var lstContact = _ContactReporitory.GetAll();
            
            var totalContact = lstContact.Count();
            lstContact = lstContact.Skip((page - 1) * Webconfig.RowLimit).Take(Webconfig.RowLimit).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Contact/_ListData.cshtml", lstContact),
                totalPages = Math.Ceiling(((double)totalContact / Webconfig.RowLimit)),
            }, JsonRequestBehavior.AllowGet);
        }

        [Authorize(Roles = "Edit")]
        public ActionResult Answer(int id)
        {
            var objQuestion = _ContactReporitory.Find(id);
            return Json(RenderViewToString("~/Areas/Admin/Views/Contact/_Answer.cshtml", objQuestion), JsonRequestBehavior.AllowGet);
        }

       

        [Authorize(Roles = "Delete")]
        public ActionResult Delete(int id)
        {
            try
            {
                var obj = _ContactReporitory.Find(id);
                _ContactReporitory.Delete(id);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Xóa câu hỏi thành công",
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Xóa câu hỏi thất bại",
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
                    var obj = _ContactReporitory.Find(Convert.ToInt32(item));
                    _ContactReporitory.Delete(Convert.ToInt32(item));
                   
                    count++;
                }
                catch (Exception)
                {
                    continue;
                }
            }
            return Json(new
            {
                Messenger = string.Format("Xóa thành công {0} câu hỏi", count),
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
