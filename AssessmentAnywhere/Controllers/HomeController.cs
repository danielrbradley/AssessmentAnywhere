namespace AssessmentAnywhere.Controllers
{
    using System.Web.Mvc;

    public class HomeController : Controller
    {
        // GET: /
        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {
                return this.RedirectToAction("Index", "Assessments");
            }

            return this.View();
        }
    }
}
