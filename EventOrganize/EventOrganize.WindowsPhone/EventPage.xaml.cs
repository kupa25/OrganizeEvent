using System;
using System.Diagnostics;
using Windows.Phone.UI.Input;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
            App.JoinedEventName = lblEventName.Text = Helper.Utility.GetValueFromCloud("EventName");

            if (string.Compare(Helper.Utility.GetValueFromCloud("PartofEvent"), App.JoinedEventName, StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                txtJoinID.Visibility = Visibility.Collapsed;
                btnJoin.Visibility = Visibility.Collapsed;
                btnLogout.Visibility = Visibility.Visible;
                
                txtEventStuff.Text = "Event Info\nSomething happened\n\n\nAnd it failed";
                //Make it visible only if its the leader visiting
                var isleader = string.Compare(Helper.Utility.GetValueFromCloud("LeaderID"), Helper.Utility.GetValueFromCloud("AzureLeaderID"), StringComparison.CurrentCultureIgnoreCase) == 0;

                btnleader.Visibility = txtLeaderMsg.Visibility = isleader ? Visibility.Visible : Visibility.Collapsed;
            }
            else
            {
                txtJoinID.Visibility = Visibility.Visible;
                btnJoin.Visibility = Visibility.Visible;
                btnLogout.Visibility = Visibility.Collapsed;
                btnleader.Visibility = txtLeaderMsg.Visibility = Visibility.Collapsed;

                txtEventStuff.Text = "";
            }

            
            Debug.WriteLine("Registering for Event: " + App.JoinedEventName);
        }

        //Join button
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            //check whether the join id matches what we selected.
            var confirmJoinId = Helper.Utility.GetValueFromCloud("JOINID");
            if (string.Compare(txtJoinID.Text, confirmJoinId, StringComparison.CurrentCultureIgnoreCase) == 0)
            {
                Helper.Utility.AddToCloud("PartofEvent", App.JoinedEventName);

                //Get the current page ready
                txtJoinID.Visibility = Visibility.Collapsed;
                btnJoin.Visibility = Visibility.Collapsed;
                btnLogout.Visibility = Visibility.Visible;

                txtEventStuff.Text = "Event Info\nSomething happened\n\n\nAnd it failed";

                //Make it visible only if its the leader visiting
                var isleader = string.Compare(Helper.Utility.GetValueFromCloud("LeaderID"), Helper.Utility.GetValueFromCloud("AzureLeaderID"), StringComparison.CurrentCultureIgnoreCase) == 0;
                btnleader.Visibility = txtLeaderMsg.Visibility = isleader ? Visibility.Visible : Visibility.Collapsed;

                //Update the appropriate tags for this event
                Helper.Utility.CreateAndUpdateTags();
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

        //Logout button
        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            Helper.Utility.RemoveFromCloud("JOINID");
            Helper.Utility.RemoveFromCloud("PartofEvent");
            
            App.JoinedEventName = null;
            //Update the appropriate tags for this event
            Helper.Utility.CreateAndUpdateTags();

            Frame.Navigate(typeof (MainPage));
        }

        private void txtleaderSend_Click(object sender, RoutedEventArgs e)
        {
            //send a message to everyone listening.

            EventOrganizePush.NotifyGroupUser(txtLeaderMsg.Text, App.JoinedEventName);

        }
    }
}
