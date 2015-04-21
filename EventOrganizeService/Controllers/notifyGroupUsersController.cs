using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using Microsoft.ServiceBus.Notifications;
using Microsoft.WindowsAzure.Mobile.Service;
using Newtonsoft.Json.Linq;

namespace EventOrganizeService.Controllers
{
    public class NotifyGroupUsersController : ApiController
    {
        public ApiServices Services { get; set; }

        // GET api/NotifyAllUsers
        public string Get()
        {
            Services.Log.Info("Hello from custom controller!");
            return "Hello";
        }

        // The following call is for illustration purpose only. The function
        // body should be moved to a controller in your app where you want
        // to send a notification.
        public async Task<bool> Post(JObject data)
        {
            try
            {
                //Store
                NotificationHubClient hub = NotificationHubClient.CreateClientFromConnectionString(
                    "Endpoint=sb://eventorganize.servicebus.windows.net/;SharedAccessKeyName=DefaultFullSharedAccessSignature;SharedAccessKey=R+Ak2hQnH+5wEclyguFm60dLxpOaFYQ6JJLefNoZuyo=",
                    "eventorganize");

                var eventName = string.Empty;
                var message = string.Empty;
                if (data["EventName"] != null)
                {
                    eventName = data["EventName"].ToString();
                }

                if (data["toast"] != null)
                {
                    message = data["toast"].ToString();
                }

                var toast = string.Format("<toast><visual><binding template=\"ToastText01\"><text id=\"1\">{0}</text></binding></visual></toast>", message);

                if (string.IsNullOrEmpty(eventName))
                {
                    await hub.SendWindowsNativeNotificationAsync(toast);
                }
                else
                {
                    await hub.SendWindowsNativeNotificationAsync(toast, "Event:" + eventName);
                }

                return true;
            }
            catch (Exception e)
            {
                Services.Log.Error(e.ToString());
            }
            return false;
        }

    }
}
