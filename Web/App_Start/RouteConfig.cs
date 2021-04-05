using System.Web.Mvc;
using System.Web.Routing;

namespace Web
{
    public class RouteConfig
    {
        
        public static void RegisterRoutes(RouteCollection routes)
        {
            //routes.IgnoreRoute("{resource}.axd/{*pathInfo}");
            routes.MapRoute("search", "search", new { controller = "Product", action = "Search" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("tintuc", "tin-tuc.html", new { controller = "News", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("about", "gioi-thieu.html", new { controller = "About", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("recruitment", "tuyen-dung.html", new { controller = "Recruitment", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("home", "trang-chu.html", new { controller = "Home", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("NewDetail", "tin-tuc/{linkseo}.html", new { controller = "News", action = "Detail", linkseo = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("InstructionDetail", "huong-dan/{linkseo}.html", new { controller = "Instruction", action = "Detail", linkseo = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("ProductDetail", "detail/{linkseo}.html", new { controller = "Product", action = "Detail", linkseo = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("ProductList", "{linkseo}.html", new { controller = "Product", action = "Index", linkseo = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("Category", "danh-muc/{linkseo}", new { controller = "Category", action = "Index", linkseo = UrlParameter.Optional }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("huong-dan", "pages/huong-dan.html", new { controller = "InstructionHome", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("AddCart", "dat-mua-hang", new { controller = "Cart", action = "AddProduct" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute("Giohang", "cart/gio-hang.html", new { controller = "Cart", action = "Index" }, namespaces: new[] { "Web.Controllers" });
            routes.MapRoute(
                name: "Default",
                url: "{controller}/{action}/{id}",
                defaults: new { controller = "Home", action = "Index", id = UrlParameter.Optional },
                namespaces: new[] { "Web.Controllers" }
            );
        }
    }
}