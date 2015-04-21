using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556
using EventOrganize.Domain;

namespace EventOrganize
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AddEvent : Page
    {
        private int zipcode;

        public AddEvent()
        {
            this.InitializeComponent();

            //load the location
            loadLocation();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
            //Frame.Navigate(typeof (MainPage));

            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
                e.Handled = true;
            }

        }

        private void loadLocation()
        {
            zipcode = 32258;
            int result;
            //get the geolocation

            if (App.address != null)
            {
                int.TryParse(App.address.PostalAddress, out result);
                if (result > 0)
                {
                    zipcode = int.Parse(App.address.PostalAddress);
                }
            }

            Debug.WriteLine("ZipCode: " + zipcode);
            lblLocation.Text = "You are at " + zipcode + " zipcode";

            btnAddEvent.IsEnabled = true;
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnAddEvent_Click(object sender, RoutedEventArgs e)
        {
            //EventOrganizePush.NotifyAllUser("NotifyAlluser method invoked");
            OrganizeEvent newEvent = new OrganizeEvent
            {
                LeaderID = txtLeaderName.Text
                ,JoinID = txtEventKey.Text
                //,locationLatitude = 
                //,LocationLongitude = longitude
                ,ZipCode = zipcode
                ,Name = txtEventName.Text
            };

            Debug.WriteLine("Getting ready to Add an event");

            EventOrganizePush.AddEvent(newEvent);
            Debug.WriteLine("Event has been added go back");

            Frame frame = Window.Current.Content as Frame;
            if (frame == null)
            {
                return;
            }

            if (frame.CanGoBack)
            {
                frame.GoBack();
                //e.Handled = true;
            }
        }
    }
}
