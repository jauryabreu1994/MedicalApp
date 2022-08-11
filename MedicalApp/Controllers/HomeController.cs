using System.Web.Mvc;

namespace MedicalApp.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            //return RedirectToAction("Index", "DashBoard");
            return View();
        }

        public ActionResult Error()
        {
            return View();
        }
    }
}