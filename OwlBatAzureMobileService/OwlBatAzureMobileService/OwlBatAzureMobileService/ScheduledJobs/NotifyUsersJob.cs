using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.WindowsAzure.Mobile.Service;
using Microsoft.WindowsAzure.Mobile.Service.ScheduledJobs;
using System.Threading;

namespace OwlBatAzureMobileService.ScheduledJobs
{
    // A simple scheduled job which can be invoked manually by submitting an HTTP
    // POST request to the path "/jobs/NotifyUsers".

    public class NotifyUsersJob : ScheduledJob
    {
        protected override void Initialize(ScheduledJobDescriptor scheduledJobDescriptor, CancellationToken cancellationToken)
        {
            base.Initialize(scheduledJobDescriptor, cancellationToken);
        }

        public override async Task ExecuteAsync()
        {
            var currentUser = "MicrosoftAccount:2d8e28622da57c94";
            string wnsToast =
                string.Format(
                "<?xml version=\"1.0\" encoding=\"utf-8\"?><toast><visual><binding template=\"ToastText01\"><text id=\"1\">Hi, {0}!</text></binding></visual></toast>",
                currentUser);
            WindowsPushMessage message = new WindowsPushMessage();
            message.XmlPayload = wnsToast;
            await Services.Push.SendAsync(message, currentUser);

            //return await Task.FromResult(true);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
            }
        }
    }
}