using Element.JobSchedulerUI.Extensions;
using System.Web.Http;

namespace Element.JobSchedulerUI.Controllers
{
    [RoutePrefix("job")]
    public class JobsController : ApiController
    {
        [HttpGet]
        [Route("all")]
        public IHttpActionResult GetAll()
        {
            return Ok(BackgroundJob.Instance?.GetJobs());
        }

        [HttpPost]
        [Route("{job}/trigger")]
        public IHttpActionResult Trigger(string job)
        {
            BackgroundJob.Instance?.Trigger(job);

            return Ok();
        }

        [HttpPost]
        [Route("{job}/cancel")]
        public IHttpActionResult Cancel(string job)
        {
            BackgroundJob.Instance?.Cancel(job);

            return Ok();
        }
    }
}
