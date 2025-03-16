using Microsoft.AspNetCore.Mvc;

namespace Siyasett.Web.Controllers
{
    public class AgendaController : Controller
    {

        [Route("gundem")]

        public IActionResult Index()
        {
            return View();
        }
    }
}
