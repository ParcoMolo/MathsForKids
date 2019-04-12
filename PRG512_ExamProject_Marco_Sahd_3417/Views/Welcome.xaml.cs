using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using PRG512_ExamProject_Marco_Sahd_3417.Models;
using PRG512_ExamProject_Marco_Sahd_3417.Utils;
using System.Diagnostics;
using PRG512_ExamProject_Marco_Sahd_3417.Classes;
using System.IO;
using Newtonsoft.Json;
using Windows.UI.Xaml.Media;
using Windows.UI;

namespace PRG512_ExamProject_Marco_Sahd_3417.Views
{
    public sealed partial class Welcome : Page
    {
        private Account _activeAccount;
        public List<EGrades> _myfuckingGrades = Enum.GetValues(typeof(EGrades)).Cast<EGrades>().ToList();
        public List<EDifficulty> _myIntenseFuckingDiffs = Enum.GetValues(typeof(EDifficulty)).Cast<EDifficulty>().ToList();
        public bool IsThisPractice;

        public Welcome()
        {
            InitializeComponent();
            Setup();

            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            ColumnDefinition gridCol3 = new ColumnDefinition();
            ColumnDefinition gridCol4 = new ColumnDefinition();
            SaltyResulty.ColumnDefinitions.Add(gridCol1);
            SaltyResulty.ColumnDefinitions.Add(gridCol2);
            SaltyResulty.ColumnDefinitions.Add(gridCol3);
            SaltyResulty.ColumnDefinitions.Add(gridCol4);
            SaltyResulty.Background = new SolidColorBrush(Colors.Gray);
        }

        void Setup()
        {
            ChooseGrade.ItemsSource = _myfuckingGrades;
            ChooseGrade.SelectedItem = _myfuckingGrades.FirstOrDefault();
            ChooseIntensity.ItemsSource = _myIntenseFuckingDiffs;
            ChooseIntensity.SelectedItem = _myIntenseFuckingDiffs.FirstOrDefault();
            IsThisPractice = true;

        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _activeAccount = (Account)e.Parameter;
            if (_activeAccount != null)
            {
                UserNameText.Text = _activeAccount.Username;
            }

            List<Results> myShittyWokResults = new List<Results>();
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;
            //if results file exists
            if (File.Exists($"{localFolder.Path}\\results.json"))
            {
                // Read the file and display it line by line.  
                var lines = File.ReadLines($"{localFolder.Path}\\results.json");
                foreach (var line in lines)
                {
                    myShittyWokResults.Add(JsonConvert.DeserializeObject<Results>(line));
                }
            }
            
        
            //I think the problem lies here with this foreach statement, it adds the same name to the list if the username exists. Thus resulting in duplicates when viewing the existing userlist.
            var counter = 0;
            myShittyWokResults.ForEach(_result =>
                {
                    if (_result.Person.Name == _activeAccount.Username)
                    {
                        RowDefinition gridRow1 = new RowDefinition();
                        gridRow1.Height = new GridLength(45);
                        SaltyResulty.RowDefinitions.Add(gridRow1);

                        TextBlock resultBlock = new TextBlock();
                        resultBlock.FontSize = 16;
                        resultBlock.Width = 150;
                        resultBlock.Height = 25;
                        resultBlock.VerticalAlignment = VerticalAlignment.Center;
                        resultBlock.Name = $"";
                        resultBlock.Text = _result.TestResults;

                        Grid.SetColumn(resultBlock, 0);
                        Grid.SetRow(resultBlock, counter);
                        SaltyResulty.Children.Add(resultBlock);

                        TextBlock personBlock = new TextBlock();
                        personBlock.FontSize = 16;
                        personBlock.Width = 150;
                        personBlock.Height = 25;
                        personBlock.VerticalAlignment = VerticalAlignment.Center;
                        personBlock.Name = $"";
                        personBlock.Text = _result.Person.Name;
                        
                        Grid.SetColumn(personBlock, 1);
                        Grid.SetRow(personBlock, counter);
                        SaltyResulty.Children.Add(personBlock);

                        TextBlock gradeBlock = new TextBlock();
                        gradeBlock.FontSize = 16;
                        gradeBlock.Width = 150;
                        gradeBlock.Height = 25;
                        gradeBlock.VerticalAlignment = VerticalAlignment.Center;
                        gradeBlock.Name = $"";
                        gradeBlock.Text = _result.Grade.ToString();

                        Grid.SetColumn(gradeBlock, 2);
                        Grid.SetRow(gradeBlock, counter);
                        SaltyResulty.Children.Add(gradeBlock);

                        TextBlock diffBlock = new TextBlock();
                        diffBlock.FontSize = 16;
                        diffBlock.Width = 150;
                        diffBlock.Height = 25;
                        diffBlock.VerticalAlignment = VerticalAlignment.Center;
                        diffBlock.Name = $"";
                        diffBlock.Text = _result.Difficulty.ToString();

                        Grid.SetColumn(diffBlock, 3);
                        Grid.SetRow(diffBlock, counter);
                        SaltyResulty.Children.Add(diffBlock);
                        counter++;
                    }
                });

        }

        private void Button_Forget_User_Click(object sender, RoutedEventArgs e)
        {
            // Remove it from Microsoft Passport
            MicrosoftPassportHelper.RemovePassportAccountAsync(_activeAccount);

            // Remove it from the local accounts list and resave the updated list
            AccountHelper.RemoveAccount(_activeAccount);

            Debug.WriteLine("User " + _activeAccount.Username + " deleted.");

            // Navigate back to UserSelection page. (changed it to login page)
            Frame.Navigate(typeof(Login));
        }

        private void generateTesticals_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var MyTestParams = new TestParams()
                {
                    Difficulty = (EDifficulty)ChooseIntensity.SelectedValue,
                    Grade = (EGrades)ChooseGrade.SelectedValue,
                    Questions = Convert.ToInt32(AmountOfMammaQ.Text),
                    Person = new Person() { Name = _activeAccount.Username },
                    ThisIsPractice = IsThisPractice.Equals(PracticeOrNot.IsChecked)
                };

                MyHackoWackoSnacko sweezieMcNeezie = new MyHackoWackoSnacko()
                {
                    Account = _activeAccount,
                    TestParams = MyTestParams
                };

                this.Frame.Navigate(typeof(QuestionPage), sweezieMcNeezie);
            }
            catch (Exception ex)
            {

            }
        }
    }
}
