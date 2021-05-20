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
            var model = this.GetAllJobs().ToList();
            return View(model);
        }

        public ActionResult Trigger(string job)
        {
            this.TriggerJob(job);

            return Redirect("~/");
        }

        public ActionResult Cancel(string job)
        {
            this.CancelJob(job);

            return Redirect("~/");
        }
    }
}