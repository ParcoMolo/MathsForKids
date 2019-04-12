using PRG512_ExamProject_Marco_Sahd_3417.Classes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI;
using PRG512_ExamProject_Marco_Sahd_3417.Views;
using Newtonsoft.Json;
using System.IO;
using Windows.Storage;
using System.Threading.Tasks;


// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace PRG512_ExamProject_Marco_Sahd_3417.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>

    public partial class QuestionPage : Page
    {
        private TestParams _testParams;
        private Test _generatedTest;
        private MyHackoWackoSnacko _myHacko;
        public QuestionPage()
        {
            this.InitializeComponent();

            // Create Columns

            ColumnDefinition gridCol1 = new ColumnDefinition();
            ColumnDefinition gridCol2 = new ColumnDefinition();
            ColumnDefinition gridCol3 = new ColumnDefinition();
            QNA.ColumnDefinitions.Add(gridCol1);
            QNA.ColumnDefinitions.Add(gridCol2);
            QNA.ColumnDefinitions.Add(gridCol3);
            QNA.Background = new SolidColorBrush(Colors.Gray);

        }

        private async void GenerateTest(TestParams testParams)
        {
            List<string> questions = new List<string>();
            List<string> answers = new List<string>();

            //adding list to store user answers
            List<string> userAnswers = new List<string>();

            var Randy = new RandyMandy();
            var performanceMeter = new Stopwatch();
            performanceMeter.Start();

            var results = await Randy.GenerateThisTest(testParams);
            results.ForEach(qna =>
            {
                questions.Add(qna.Item1);
                answers.Add(qna.Item2);
            });

            performanceMeter.Stop();
            Debug.WriteLine($"{performanceMeter.Elapsed.TotalSeconds} time taken to be AMAZING");

            _generatedTest = new Test()
            {
                Person = testParams.Person,
                Grade = testParams.Grade,
                Difficulty = testParams.Difficulty,
                IsPracticeMode = testParams.ThisIsPractice,
                Questions = questions,
                Answers = answers,
            };


            var counter = 0;
            questions.ForEach(_question =>
            {
                RowDefinition gridRow1 = new RowDefinition();
                gridRow1.Height = new GridLength(45);
                QNA.RowDefinitions.Add(gridRow1);

                TextBlock questionBlock = new TextBlock();
                questionBlock.FontSize = 16;
                questionBlock.Width = 100;
                questionBlock.Height = 25;
                questionBlock.VerticalAlignment = VerticalAlignment.Center;
                questionBlock.Text = $"{counter + 1}) {_question} = ";
                questionBlock.Name = $"questionBlock_{counter}";

                TextBox answerBox = new TextBox();
                answerBox.FontSize = 16;
                answerBox.Width = 100;
                answerBox.Height = 25;
                answerBox.VerticalAlignment = VerticalAlignment.Center;
                answerBox.Name = $"answerBox_{counter}";

                Button showMeTheMoney = new Button();
                showMeTheMoney.FontSize = 12;
                showMeTheMoney.Width = 100;
                showMeTheMoney.Height = 25;
                showMeTheMoney.Content = "Show Answer";
                showMeTheMoney.VerticalAlignment = VerticalAlignment.Center;
                showMeTheMoney.Name = $"showMeTheMoney_{counter}";
                showMeTheMoney.Click += showMeTheMoney_clicked;

                Grid.SetColumn(questionBlock, 0);
                Grid.SetRow(questionBlock, counter);
                QNA.Children.Add(questionBlock);


                Grid.SetColumn(answerBox, 1);
                Grid.SetRow(answerBox, counter);
                QNA.Children.Add(answerBox);

                if (_generatedTest.IsPracticeMode)
                {
                    Grid.SetColumn(showMeTheMoney, 2);
                    Grid.SetRow(showMeTheMoney, counter);
                    QNA.Children.Add(showMeTheMoney);
                }
                counter++;
            });
        }

        private void showMeTheMoney_clicked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            var myButton = ((Button)sender).Name;
            //returns two values in array 0 = name of control  1 = controlIDNumber
            var myChosenItem = myButton.Split('_');
            var answer = _generatedTest.Answers[Convert.ToInt32(myChosenItem[1])];
            TextBox TheTextBoxYaChecking = (TextBox)QNA.FindName($"answerBox_{myChosenItem[1]}");
            // GET MY VALUE ::::  var bobbys_answer = TheTextBoxYaChecking.Text;
            TheTextBoxYaChecking.Text = answer;
            Debug.WriteLine(answer);
        }



        private async void ImDoneBitches_Click(object sender, RoutedEventArgs e)
        {
            var Happyness = 0;

            var amountofQs = _generatedTest.Questions.Count;

            for (int i = 0; i < amountofQs; i++)
            {
                var controller = (TextBox)QNA.FindName($"answerBox_{i}");
                var fuckingUsersAnswer = controller.Text;
                if (fuckingUsersAnswer == _generatedTest.Answers[i])
                {
                    Happyness++;
                }
            }

            var Results = new Results()
            {
                Difficulty = _generatedTest.Difficulty,
                Grade = _generatedTest.Grade,
                Person = _generatedTest.Person,
                TestResults = $"{Happyness}/{_generatedTest.Questions.Count}"
            };

            ///
            var myResults = JsonConvert.SerializeObject(Results);
            await WriteResults(myResults);

            Frame.Navigate(typeof(Welcome), _myHacko.Account);
        }

        public async Task<bool> WriteResults(string jsonString)
        {
            Windows.Storage.StorageFolder localFolder = Windows.Storage.ApplicationData.Current.LocalFolder;

            if (!File.Exists($"{localFolder.Path}\\results.json"))
            {
                await localFolder.CreateFileAsync("results.json");
            }

            var file = await localFolder.GetFileAsync("results.json");
            await FileIO.AppendTextAsync(file, jsonString +"\n");

            return true;
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            _myHacko = (MyHackoWackoSnacko)e.Parameter;
            _testParams = _myHacko.TestParams;
            GenerateTest(_testParams);
            BackButton.IsEnabled = this.Frame.CanGoBack;
        }

        private Modifier GetTestModifier(TestParams testParams)
        {
            switch (testParams.Grade)
            {
                case EGrades.One:
                    return new LevelOne().Modifier;
                case EGrades.Two:
                    return new LevelTwo().Modifier;
                case EGrades.Three:
                    return new LevelThree().Modifier;
                case EGrades.Four:
                    return new LevelFour().Modifier;
                default:
                    return new LevelOne().Modifier;
            }
        }



        private void Back_Click(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            On_BackRequested();
        }

        // Handles system-level BackRequested events and page-level back button Click events
        private bool On_BackRequested()
        {
            if (this.Frame.CanGoBack)
            {
                this.Frame.GoBack();
                return true;
            }
            return false;
        }


    }
}
