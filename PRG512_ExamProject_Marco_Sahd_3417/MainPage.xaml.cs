using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using PRG512_ExamProject_Marco_Sahd_3417.Views;
using PRG512_ExamProject_Marco_Sahd_3417.Utils;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

namespace PRG512_ExamProject_Marco_Sahd_3417
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            Loaded += MainPage_Loaded;
        }

        private async void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            // Load the local Accounts List before navigating to the UserSelection page
            await AccountHelper.LoadAccountListAsync();
            Frame.Navigate(typeof(Login));
        }
    }
}
