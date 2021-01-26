using System.Web.Mvc;

namespace StefaniniPruebaTecnica.WebApi.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        public ActionResult Index()
        {
            return View();
        }
    }
}