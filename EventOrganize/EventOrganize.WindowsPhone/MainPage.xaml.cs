using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Devices.Geolocation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using EventOrganize.Domain;
using Microsoft.WindowsAzure.MobileServices;
using Newtonsoft.Json.Linq;

namespace EventOrganize
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Geolocator _geolocator = null;
        private double lat;
        private double longitude;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            _geolocator = new Geolocator();
            _geolocator.DesiredAccuracy = PositionAccuracy.High;
            _geolocator.MovementThreshold = 10;
            _geolocator.PositionChanged += _geolocator_PositionChanged;

        }

        private void _geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            lat = args.Position.Coordinate.Latitude;
            longitude = args.Position.Coordinate.Longitude;

            //Do something with this.
            Helper.Utility.CreateAndUpdateTags(lat, longitude);
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // TODO: Prepare page for display here.

            // TODO: If your application contains multiple pages, ensure that you are
            // handling the hardware Back button by registering for the
            // Windows.Phone.UI.Input.HardwareButtons.BackPressed event.
            // If you are using the NavigationHelper provided by some templates,
            // this event is handled for you.
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            //get the geolocation

            //EventOrganizePush.NotifyAllUser("NotifyAlluser method invoked");
            OrganizeEvent newEvent = new OrganizeEvent
            {
                LeaderID = 2
                ,JoinID = "ABCD"
                ,locationLatitude = lat
                ,LocationLongitude = longitude
            };

            Debug.WriteLine("Passing the event to the mobile service to add");

            EventOrganizePush.AddEvent(newEvent);

        }
    }
}
