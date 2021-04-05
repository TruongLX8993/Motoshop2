using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AttributeRouting.Web.Mvc;
using Web.BaseSecurity;
using Web.Controllers;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class HomeController : BaseController
    {
        readonly IUserAdminRepository _userRepository = new UserAdminRepository();
        readonly INewsRepository _newsRepository = new NewsRepository();
        //
        // GET: /Admin/Home/
        [Authorize]
        public ActionResult Index()
        {
            if (User == null)
            {
                return RedirectToAction("Login", "Account");
            }
            if (User.UserType == (int)EnumHelper.UserType.Binhthuong)
            {
                return RedirectToAction("AccessDenined", "Error");
            }
            // Thông tin đăng nhập User:
            var currentUser = _userRepository.Find(User.ID);
            ViewBag.rowUser = currentUser;
            // List danh sach Tin bai theo trang thai
            if (User.isAdmin == false)
            {
                ViewBag.lstQuyenId = currentUser.QuyTrinhXuatBanID.Split(',').ToList();
            }
            
            return View();
        }
    }
}
