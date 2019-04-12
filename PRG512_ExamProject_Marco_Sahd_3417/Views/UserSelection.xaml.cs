using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using System.Diagnostics;
using PRG512_ExamProject_Marco_Sahd_3417.Models;
using PRG512_ExamProject_Marco_Sahd_3417.Utils;

namespace PRG512_ExamProject_Marco_Sahd_3417.Views
{    
    public sealed partial class UserSelection : Page
    {
        public UserSelection()
        {
            InitializeComponent();
            Loaded += UserSelection_Loaded;
        }

        private void Back_Click(object sender, RoutedEventArgs e)
        {
            this.Frame.GoBack();
        }


        
        private void UserSelection_Loaded(object sender, RoutedEventArgs e)
        {
            if (AccountHelper.AccountList.Count == 0)
            {
                //If there are no accounts navigate to the LoginPage
                Frame.Navigate(typeof(Login));
            }
            
            UserListView.ItemsSource = AccountHelper.AccountList;
            UserListView.SelectionChanged += UserSelectionChanged;
        }

        /// <summary>
        /// Function called when an account is selected in the list of accounts
        /// Navigates to the Login page and passes the chosen account
        /// </summary>
        private void UserSelectionChanged(object sender, RoutedEventArgs e)
        {
            if (((ListView)sender).SelectedValue != null)
            {
                Account account = (Account)((ListView)sender).SelectedValue;
                if (account != null)
                {
                    Debug.WriteLine("Account " + account.Username + " selected!");
                }
                /*Removed the direct login to account when different user is selected in order to avoid some of the security risks that this view has.
                I will have to change this later on so that when a different user is selected then it just calls the authentication function asking 
                the user to enter their password again.
                 */
                Frame.Navigate(typeof(Login)/*, account*/); 
            }
        }

        /// <summary>
        /// Function called when the "+" button is clicked to add a new user.
        /// Navigates to the Login page with nothing filled out
        /// </summary>
        private void AddUserButton_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(Login));
        }
    }
}
