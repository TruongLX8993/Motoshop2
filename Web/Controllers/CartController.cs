using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using Web.Model;
using Web.Model.CustomModel;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductRepository productRepository = new ProductRepository();
        private readonly IOrderRepository orderRepository = new OrderRepository();
        private readonly IOrderDetailRepository orderDetailRepository = new OrderDetailRepository();
       
        public ActionResult Index()
        {
            var lst = TempData["CartItem"];
            string orderModel = "";
            if (lst != null)
            {
                orderModel = JsonConvert.SerializeObject(lst);
            }
            ViewBag.ListCart = orderModel;
            return View();
        }

        public ActionResult AddProduct(int id, int quantity,string cartitem)
        {
            var lst = new List<CartItem>();
            var product = productRepository.Find(id); 
            if (!string.IsNullOrEmpty(cartitem))
            {
                var lstSession = JsonConvert.DeserializeObject<List<CartItem>>(cartitem);
                if (lstSession.Exists(x => x.Product.ID == id))
                {
                    foreach (var item in lst)
                    {
                        if (item.Product == product)
                        {
                            item.Quantity += quantity;
                        }
                    }
                }
                else
                {
                    var item = new CartItem();
                    item.Product = product;
                    item.Quantity = quantity;
                    lst.Add(item);
                    lst.AddRange(lstSession);
                }
            }
            else
            {
                var item = new CartItem();
                item.Product = product;
                item.Quantity = quantity;
                lst.Add(item);
            }
            TempData["CartItem"] = lst;
            return RedirectToAction("Index");
        }
        

        [HttpPost]
        public ActionResult Order(string customerOrder, string orderDetail)
        {
            var order = new JavaScriptSerializer().Deserialize<tbl_Order>(customerOrder);
            var details = new JavaScriptSerializer().Deserialize<List<OrderDetail>>(orderDetail);
            try
            {
                order.Status = 1;
                order.CreatedDate = DateTime.Now;
                int id = orderRepository.Add(order);
                foreach (var item in details)
                {
                    item.OrderID = id;
                    orderDetailRepository.Add(item);
                }
                return Json(new
                {
                    Success = true
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    Success = false
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}