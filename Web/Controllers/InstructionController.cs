using System.Linq;
using System.Web.Mvc;
using Web.BaseSecurity;
using Web.Repository.Entity;
using Web.Repository;

namespace Web.Controllers
{
    public class InstructionController : BaseController
    {
        readonly IInstructionRepository _instructionRepository = new InstructionRepository();

        public ActionResult Detail(string linkseo)
        {
            var obj = _instructionRepository.GetAll().FirstOrDefault(x => x.LinkSeo.Equals(linkseo));
            return View(obj);
        }
    }
}