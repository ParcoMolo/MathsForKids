using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using PRG512_ExamProject_Marco_Sahd_3417.Utils;
using PRG512_ExamProject_Marco_Sahd_3417.Models;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PRG512_ExamProject_Marco_Sahd_3417.Views
{
    public sealed partial class Login : Page
    {
        private Account _account;
        private bool _AmIForeign;

        public Login()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Function called when this frame is navigated to.
        /// Checks to see if Microsoft Passport is available and if an account was passed in.
        /// If an account was passed in set the "_isExistingAccount" flag to true and set the _account
        /// </summary>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            // Check Microsoft Passport is setup and available on this machine
            if (await MicrosoftPassportHelper.MicrosoftPassportAvailableCheckAsync())
            {
                //how the fuck wont e.Parameter be null
                if (e.Parameter != null)
                {
                    _AmIForeign = true;
                    // Set the account to the existing account being passed in
                    _account = (Account)e.Parameter;
                    UsernameTextBox.Text = _account.Username;
                    SignInPassport();
                }
                //end up doing nothing
            }
            else
            {
                // Microsoft Passport is not setup so inform the user
                PassportStatus.Background = new SolidColorBrush(Windows.UI.Color.FromArgb(255, 50, 170, 207));
                PassportStatusText.Text = "Microsoft Passport is not setup!\n" +
                    "Please go to Windows Settings and set up a PIN to use it.";
                PassportSignInButton.IsEnabled = false;
            }
        }

        private void PassportSignInButton_Click(object sender, RoutedEventArgs e)
        {
            ErrorMessage.Text = "";
            SignInPassport();
        }

        private void RegisterButtonTextBlock_OnPointerPressed(object sender, PointerRoutedEventArgs e)
        {
            ErrorMessage.Text = "";
            Frame.Navigate(typeof(PassportRegister));
        }

        private async void SignInPassport()
        {

            if (!(UsernameTextBox.Text.Length > 0))
            {
                ErrorMessage.Text = "Dude....";
                return;
            }

            bool userNameExistsInFile = await AccountHelper.IsInFuckingFile(UsernameTextBox.Text);

            if (_AmIForeign)
            {
                //Pinsly
                if (await MicrosoftPassportHelper.GetPassportAuthenticationMessageAsync(_account))
                {
                    Frame.Navigate(typeof(Welcome), _account);
                }
            }
            else if (userNameExistsInFile)
            {
                var amilegit = await MicrosoftPassportHelper.CreatePassportKeyAsync(UsernameTextBox.Text);
                if (amilegit)
                {
                    ////Register a new account
                    _account = AccountHelper.AddAccount(UsernameTextBox.Text);
                    //Navigate to the Welcome Screen. 
                    Frame.Navigate(typeof(Welcome), _account);
                }
                else
                {
                    ErrorMessage.Text = "Error Signing in";
                }
            }
            else
            {
                ErrorMessage.Text = "Yo homie, we don't know you";
            }
        }

    }
}
