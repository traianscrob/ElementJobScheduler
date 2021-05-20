using Element.JobScheduler.Models;

namespace Element.JobSchedulerUI.Models
{
    public class JobModelView : JobModel
    {
        public string Action => IsCancelling ? "Cancelling" : IsRunning ? "Cancel" : "Trigger";
        public bool Enable => !IsCancelling;
        public string ImageUrl =>
            LastCancelledDate.HasValue && LastCancelledDate > (LastRunDate ?? System.DateTime.MinValue)
            ? "~/Content/Images/canceled.png"
            : RunWithSuccess.HasValue
                ? RunWithSuccess.Value
                    ? "~/Content/Images/success.png"
                    : "~/Content/Images/failed.png"
                : string.Empty;
    }
}