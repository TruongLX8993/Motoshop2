using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml.Linq;
using Web.BaseSecurity;
using Web.Core;
using Web.Model;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Areas.Admin.Controllers
{ 
    public class ProductController : BaseController
    {
       private readonly  IProductRepository productRepository = new ProductRepository();
       private readonly  ICategoryRepository categoryRepository = new CategoryRepository();

        // GET: Product
        [Authorize]
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ListData(string keyWord, int pageIndex)
        {
            var model = productRepository.ListAll(0, keyWord).ToList();
            var totalAdv = model.Count();
            model = model.Skip((pageIndex - 1) * 10).Take(10).ToList();
            TempData["Page"] = pageIndex;
            return Json(new
            {
                viewContent = RenderViewToString("~/Areas/Admin/Views/Product/ListData.cshtml", model),
                totalPages = Math.Ceiling(((double)totalAdv / 10)),
            }, JsonRequestBehavior.AllowGet);
        }
        [HttpGet]
        public ActionResult AddImg(int id)
        {
            ViewBag.ID = id;
            return View();
        }
        [HttpPost]
        public JsonResult SaveImages(string images,int id)
        {
            try
            {
                JavaScriptSerializer serializer = new JavaScriptSerializer();
                var lstImages = serializer.Deserialize<List<string>>(images);
                XElement xElement = new XElement("Images");
                foreach (var item in lstImages)
                {
                    xElement.Add(new XElement("Image", item));
                }

                return Json(new
                {
                    result = true
                });
            }
            catch (Exception)
            {
                return Json(new
                {
                    result = false
                });
            }
           
        }
        [HttpGet]
        public ActionResult Add()
        {
            TempData["Category"] = categoryRepository.GetAll().ToList();
            return View();
        }
        [HttpPost]
        public ActionResult Add(Product product, string isContinue)
        {
            try
            {
                var obj = productRepository.GetAll().FirstOrDefault(x=>x.Name == product.Name);
                if (obj != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên sản phẩm đã tồn tại"
                    }, JsonRequestBehavior.AllowGet);
                }
                product.CreatedDate = DateTime.Now;
                product.LinkSeo = HelperString.RenderLinkSeo(product.Name);
                productRepository.Add(product);
                return Json(new
                {
                    IsContinue = isContinue,
                    IsSuccess = true,
                    Messenger = "Thêm mới thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Thêm mới thất bại"
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            TempData["Category"] = categoryRepository.GetAll().ToList();
            var obj = productRepository.Find(id);
            return View(obj);
        }

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            try
            {
                var obj = productRepository.GetAll().FirstOrDefault(x => x.Name == product.Name && x.ID != product.ID);
                if (obj != null)
                {
                    return Json(new
                    {
                        IsSuccess = false,
                        Messenger = "Tên sản phẩm đã tồn tại"
                    }, JsonRequestBehavior.AllowGet);
                }
                product.LinkSeo = HelperString.RenderLinkSeo(product.Name);
                productRepository.Edit(product);
                return Json(new
                {
                    IsSuccess = true,
                    Messenger = "Cập nhật thành công"
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    IsSuccess = false,
                    Messenger = "Cập nhật thất bại"
                }, JsonRequestBehavior.AllowGet);
            }
        }
        public ActionResult Delete(int id)
        {
            try
            {
                productRepository.Delete(id);
                return Json(new { IsSuccess = true, Messenger = "Xóa thành công" }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception e)
            {
                return Json(new { IsSuccess = false, Messenger = "Xóa thất bại" }, JsonRequestBehavior.AllowGet);
            }
        } 
    }
}
