using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Windows.Networking.PushNotifications;
using EventOrganize.Domain;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

// http://go.microsoft.com/fwlink/?LinkId=290986&clcid=0x409

namespace EventOrganize
{
    public class EventOrganizePush
    {
        public static PushNotificationChannel channel;

        public async static void UploadChannel()
        {
            channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();

            try
            {
                await App.EventOrganizeClient.GetPush().RegisterNativeAsync(channel.Uri);

                await App.EventOrganizeClient.InvokeApiAsync("notifyAllUsers", new JObject(new JProperty("toast", "Welcome... To the organizer")));

                Debug.WriteLine("Upload channel method is complete");
            }
            catch (Exception exception)
            {
                HandleRegisterException(exception);
            }
        }

        public async static void NotifyAllUser(string message)
        {
            try
            {
                await App.EventOrganizeClient.GetPush().RegisterNativeAsync(channel.Uri);
                await App.EventOrganizeClient.InvokeApiAsync("notifyAllUsers", new JObject(new JProperty("toast", message)));

                Debug.WriteLine("NotifyAllUser method is complete");
            }
            catch (Exception exception)
            {
                HandleRegisterException(exception);
            }
        }

        public async static void AddEvent(OrganizeEvent eventToAdd)
        {
            try
            {
                await App.EventOrganizeClient.GetTable<OrganizeEvent>().InsertAsync(eventToAdd);

                Debug.WriteLine("Event has been added");
            }
            catch (Exception exception)
            {
                HandleRegisterException(exception);
            }
        }

        private static void HandleRegisterException(Exception exception)
        {
            Debug.WriteLine("********** Exception occured **********");
            Debug.WriteLine(exception.Message);
            Debug.WriteLine(exception.StackTrace);
        }

        public static async void UpdateAzureTags(List<string> tags)
        {
            channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
            await App.EventOrganizeClient.GetPush().RegisterNativeAsync(channel.Uri, tags);
        }

        internal static async void NotifyGroupUser(string message, string eventName)
        {
            try
            {
                //channel = await PushNotificationChannelManager.CreatePushNotificationChannelForApplicationAsync();
                App.UpdateTags();
                //await App.EventOrganizeClient.GetPush().RegisterNativeAsync(channel.Uri);
                await App.EventOrganizeClient.InvokeApiAsync("notifyGroupUsers", 
                    new JObject(
                        new JProperty("toast", message),
                        new JProperty("EventName", eventName.Replace(' ', '_'))));

                Debug.WriteLine("NotifyGroup User method is complete with Toast: "+message + " And EventName: "+ eventName);
            }
            catch (Exception exception)
            {
                HandleRegisterException(exception);
            }
        }
    }
}
