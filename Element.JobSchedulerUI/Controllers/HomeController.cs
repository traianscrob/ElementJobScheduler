using Element.JobSchedulerUI.Extensions;
using Element.JobSchedulerUI.Models;
using System.Collections.Generic;
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
            return View(this.GetAllJobs() ?? new List<JobModelView>());
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