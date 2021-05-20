using Element.JobScheduler.Models;
using Element.JobSchedulerUI.Controllers;
using Element.JobSchedulerUI.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Net.Http;

namespace Element.JobSchedulerUI.Extensions
{
    public static class ApiHelper
    {
        public static IEnumerable<JobModelView> GetAllJobs(this HomeController controller)
        {
            string url = $"http://{controller.Url.RequestContext.HttpContext.Request.Url.Authority}";
            using (var client = new HttpClient(new HttpClientHandler()
            {
                UseDefaultCredentials = true,
                PreAuthenticate = true,
                AllowAutoRedirect = true,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            })
            {
                BaseAddress = new Uri(string.Format(ConfigurationManager.AppSettings["JobsApiEndPoint"], url)),
            })
            {
                var response = client.GetAsync("all").Result;
                if (response.IsSuccessStatusCode)
                {
                    var reader = new StreamReader(response.Content.ReadAsStreamAsync().Result ?? Stream.Null);
                    return JsonConvert.DeserializeObject<IEnumerable<JobModelView>>(reader.ReadToEnd());
                }

                return new List<JobModelView>();
            }
        }

        public static bool TriggerJob(this HomeController controller, string job)
        {
            string url = $"http://{controller.Url.RequestContext.HttpContext.Request.Url.Authority}";
            using (var client = new HttpClient(new HttpClientHandler()
            {
                UseDefaultCredentials = true,
                PreAuthenticate = true,
                AllowAutoRedirect = true,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            })
            {
                BaseAddress = new Uri(string.Format(ConfigurationManager.AppSettings["JobsApiEndPoint"], url)),
            })
            {
                var response = client.PostAsync($"{job}/trigger", null).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }

        public static bool CancelJob(this HomeController controller, string job)
        {
            string url = $"http://{controller.Url.RequestContext.HttpContext.Request.Url.Authority}";
            using (var client = new HttpClient(new HttpClientHandler()
            {
                UseDefaultCredentials = true,
                PreAuthenticate = true,
                AllowAutoRedirect = true,
                ClientCertificateOptions = ClientCertificateOption.Automatic
            })
            {
                BaseAddress = new Uri(string.Format(ConfigurationManager.AppSettings["JobsApiEndPoint"], url)),
            })
            {
                var response = client.PostAsync($"{job}/cancel", null).Result;
                if (response.IsSuccessStatusCode)
                {
                    return true;
                }
            }

            return false;
        }
    }
}