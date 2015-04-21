using System;
using System.Diagnostics;
using Windows.Devices.Geolocation;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using EventOrganize.Domain;
using Microsoft.WindowsAzure.MobileServices;

namespace EventOrganize
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        private Geolocator _geolocator = null;
        //private double lat;
        //private double longitude;

        public MainPage()
        {
            this.InitializeComponent();

            this.NavigationCacheMode = NavigationCacheMode.Required;
            _geolocator = new Geolocator();
            _geolocator.DesiredAccuracy = PositionAccuracy.High;
            _geolocator.MovementThreshold = 100;
            _geolocator.PositionChanged += _geolocator_PositionChanged;

            lblLoading.Visibility = Visibility.Visible;
            btnRefresh.IsEnabled = false;

        }

        private async void LoadAllLocalEvents()
        {
            MobileServiceCollection<OrganizeEvent, OrganizeEvent> items = null;
            IMobileServiceTable<OrganizeEvent> organizeMobileServiceTable = App.EventOrganizeClient.GetTable<OrganizeEvent>();
            MobileServiceInvalidOperationException exception = null;

            try
            {
                //Debug.WriteLine("Trying to load items for the zipcode: " + int.Parse(App.address.PostalAddress));

                var zipcode = 32258;

                if (App.address != null)
                {
                    Debug.WriteLine("We have an address value");
                    zipcode = int.Parse(App.address.PostalAddress);
                }
                else
                {
                    Debug.WriteLine("we don't have an address obj yet");
                }

                items =
                    await
                        organizeMobileServiceTable.Where(
                            eventItem => eventItem.ZipCode == zipcode).ToCollectionAsync();
            }
            catch (MobileServiceInvalidOperationException e)
            {
                exception = e;
            }

            if (exception != null)
            {
                await new MessageDialog(exception.Message, "Error loading items").ShowAsync();
            }
            else
            {
                lblLoading.Visibility = Visibility.Collapsed;
                ListItems.ItemsSource = items;
                this.btnAddEvent.IsEnabled = true;
            }
        }

        private async void _geolocator_PositionChanged(Geolocator sender, PositionChangedEventArgs args)
        {
            App.Lattitude = args.Position.Coordinate.Latitude;
            App.longtitude = args.Position.Coordinate.Longitude;

                //Do something with this.
                await Helper.Utility.CreateAndUpdateTags();
                Debug.WriteLine("***Done with tags about to load all the events");

            Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.High, async () =>
            {
                LoadAllLocalEvents();
                btnRefresh.IsEnabled = true;
            });
            
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

            //LoadAllLocalEvents();
        }

        private void btnTest_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof (AddEvent));
        }

        private void CheckBoxComplete_Click(object sender, RoutedEventArgs e)
        {
            var box = ((CheckBox) sender);

            //btnRefresh.IsEnabled = box.IsChecked.GetValueOrDefault(false);

            Helper.Utility.AddToCloud("JOINID", ((OrganizeEvent)box.DataContext).JoinID);
            Helper.Utility.AddToCloud("EventName", ((OrganizeEvent)box.DataContext).Name);
            Helper.Utility.AddToCloud("AzureLeaderID", ((OrganizeEvent)box.DataContext).LeaderID);

            Frame.Navigate(typeof (EventPage));
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadAllLocalEvents();
        }
    }
}
