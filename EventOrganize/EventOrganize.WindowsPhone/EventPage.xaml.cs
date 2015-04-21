using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace EventOrganize
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EventPage : Page
    {
        public EventPage()
        {
            this.InitializeComponent();
            HardwareButtons.BackPressed += HardwareButtons_BackPressed;

        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.
        /// This parameter is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!string.IsNullOrEmpty(Helper.Utility.GetValueFromCloud("PartofEvent")))
            {
                txtJoinID.Visibility = Visibility.Collapsed;
                btnJoin.Visibility = Visibility.Collapsed;
                btnLogout.Visibility = Visibility.Visible;
            }
            else
            {
                txtJoinID.Visibility = Visibility.Visible;
                btnJoin.Visibility = Visibility.Visible;
                btnLogout.Visibility = Visibility.Collapsed;
            }

            lblEventName.Text = Helper.Utility.GetValueFromCloud("EventName");
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //check whether the join id matches what we selected.
            var confirmJoinId = Helper.Utility.GetValueFromCloud("JOINID");
            if (string.Compare(txtJoinID.Text, confirmJoinId, StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                Helper.Utility.AddToCloud("PartofEvent", txtJoinID.Text);

                //Get the current page ready
                txtJoinID.Visibility = Visibility.Collapsed;
                btnJoin.Visibility = Visibility.Collapsed;
            }
            else
            {
                new MessageDialog("You didn't say the magic word", "Error").ShowAsync();
            }
        }

        private void HardwareButtons_BackPressed(object sender, BackPressedEventArgs e)
        {
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

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Helper.Utility.RemoveFromCloud("JOINID");
            Helper.Utility.RemoveFromCloud("PartofEvent");
            Frame.Navigate(typeof (MainPage));
        }
    }
}
