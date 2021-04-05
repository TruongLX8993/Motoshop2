using PagedList;
using System.Linq;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class HomeController : BaseController
    {
        private ISlideIRepository slideRepository = new SlideRepository();
        private IHomeMenuRepository menuHomeRepository = new HomeMenuRepository();
        private IProductRepository productRepository = new ProductRepository();
        public ActionResult Index(int? page)
        {
            int pagesite = 12;
            int pagenumber = (page ?? 1);
            var model = productRepository.GetAll().OrderByDescending(x => x.CreatedDate).ToPagedList(pagenumber, pagesite);
            return View(model);
        }
        public PartialViewResult Header()
        {
            return PartialView();
        }
        public PartialViewResult MenuHome()
        {
            var menu = menuHomeRepository.GetAll().ToList();
            return PartialView(menu);

        }
        public PartialViewResult Slider()
        {
            var slider = slideRepository.GetAll().ToList();
            return PartialView(slider);
        }
    }
}