namespace AssessmentAnywhere.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        // GET: /
        public ActionResult Index()
        {
            return this.RedirectToAction("Index", "Assessments");
        }
    }
}
