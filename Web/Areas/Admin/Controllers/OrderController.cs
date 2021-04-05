using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Core;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{
    public class OrderController : BaseController
    {
        private readonly IOrderRepository orderRepository = new OrderRepository();
        private readonly IOrderDetailRepository orderdetail = new OrderDetailRepository();
        private readonly IProductRepository productRepository = new ProductRepository();

        // GET: News
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult ListData(int status, string name, string tungay, string denngay, int page)
        {
            var model = orderRepository.GetAll();
            if (!string.IsNullOrEmpty(tungay) && !string.IsNullOrEmpty(denngay))
            {

                var fromDate = HelperDateTime.ConvertDate(tungay);
                var toDate = HelperDateTime.ConvertDate(denngay);
                if (fromDate > toDate)
                {
                    Session["Messenger"] = new Notified { Value = EnumNotifield.Error, Messenger = "Ngày bắt đầu phải nhỏ hơn ngày kết thúc" };
                }
                else
                {
                    model = model.Where(s => s.CreatedDate.Date.Date >= fromDate && s.CreatedDate.Date <= toDate);
                }
            }
            if (status != 0)
                model = model.Where(s => s.Status == status);
            var totalAdv = model.Count();
            model = model.Skip((page - 1) * 10).Take(10).ToList();
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Order/_ListData.cshtml", model),
                totalPages = Math.Ceiling(((double)totalAdv / 10)),
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Delete(int id)
        {
            try
            {
                orderRepository.Delete(id);
                var lstOrderDetail = orderdetail.GetAll().Where(x => x.OrderID == id).ToList();
                foreach (var item in lstOrderDetail)
                {
                    orderdetail.Delete(item.ID);
                }
                return Json(new { IsSuccess = true, Messenger = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new { IsSuccess = false, Messenger = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult Detail(int id)
        {
             var lst = orderdetail.GetAll()
                   .Join(productRepository.GetAll(),
                         o => o.ProductID,
                         p => p.ID,
                         (o, p) => new { o, p})
                   .Where(x => x.o.OrderID == id)
                   .Select(x => new {
                       x.p.Name,
                       x.p.Images,
                       x.p.Price,
                       x.p.Sale,
                       x.o.Quantity
                   }).ToList();
            List<OrderDetailModel> model = new List<OrderDetailModel>();
            foreach (var item in lst)
            {
                model.Add(new OrderDetailModel
                {
                    Name = item.Name,
                    Images = item.Images,
                    Price = item.Price,
                    Sale = item.Sale,
                    Quantity = item.Quantity    
                });
            }
            return View(model);
        }
    }
}