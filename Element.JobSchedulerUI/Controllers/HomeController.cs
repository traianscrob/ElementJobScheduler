using Element.JobScheduler.Interfaces;
using Element.JobSchedulerUI.Extensions;
using System.Linq;
using System.Web.Mvc;

namespace Element.JobSchedulerUI.Controllers
{
    public class HomeController : Controller
    {
        public HomeController()
        {
        }

        public ActionResult Index()
        {
            var jobs = BackgroundJob.Instance.GetJobs().ToList();

            return View(jobs);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}