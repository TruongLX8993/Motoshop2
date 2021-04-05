using System.Linq;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Repository;
using Web.Repository.Entity;

namespace Web.Controllers
{
    public class RecruitmentController : BaseController
    {
        private IInfoUsefulRepository infoUsefulRepository = new InfoUsefulRepository();
      
        public ActionResult Index()
        {
            var model = infoUsefulRepository.GetAll().OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            return View(model);
        }
    }
}