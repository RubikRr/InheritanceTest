using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace InheritanceTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        
        static int score = 0;
        //Ответ игрока на текущий вопрос(1,2,3,4)
        static string playerResponse = "";
        //Номер текущего вопроса
        static int questNumber=0;
        //Сколько всего вопросов
        static int totalQuestions;
        //Хранит в себе все правильные ответы
        static string correctAnswers = GetCorrectAnswers();
        //Хранит в себе все вопросы и по 4 варианта ответа на каждый
        static List<Tuple<string, List<string>>> questions = new List<Tuple<string, List<string>>>();
        public MainWindow()
        {
            InitializeComponent();
            GetAllQuestions();
            StartGame();
        }
        //Запоминает варинат ответа пользователя
        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton pressed = (RadioButton)sender;
            playerResponse = pressed.Content.ToString()[0].ToString();
        }

        //Проверка на правильность ответа
        public void IsCorrectAnswer()
        {
            string correctAns = (correctAnswers[questNumber].ToString());
            if (playerResponse == correctAns)
            {
                score++;
                scoreTest.Content = $"Score {score}/{totalQuestions}";
            }
            else
            {
                scoreTest.Content = $"Score {score}/{totalQuestions}";
            }
        }
        //Выводит след вопрос на экран
        private void nextQuest_Click(object sender, RoutedEventArgs e)
        {
            IsCorrectAnswer();
            ClearSelection();
            if (questNumber < totalQuestions - 1)
            {
                questNumber++;
                FillInTheAnswerOptions(questNumber);
            }
            else
            {
                MessageBox.Show($"Игра окончена\nВы набрали {score} o.\nЕсли вы желаете начать тест заного нажмите на кнопку \"Restart\"");
                using (StreamWriter sw = new StreamWriter(@"..\..\..\ListOfPlayers.txt", true))
                {
                    sw.WriteLine($"{playerNickname.Text} набрал {score} о.");
                }
            }

        }

        public void StartGame()
        {
            questNumber = 0;
            FillInTheAnswerOptions(questNumber);
        }
        //Обнуляет все необходимые данные для новой игры
        public void Restart()
        {
            questNumber = 0;
            score = 0;
            scoreTest.Content = $"Score";
            ClearSelection();
            playerNickname.Text = "";
            StartGame();
        }
        //Заполняет варианты ответов и сам вопрос
        public void FillInTheAnswerOptions(int questNumber)
        {
            questNumb.Text = (questNumber + 1).ToString();
            question.Content = questions[questNumber].Item1;
            var list = questions[questNumber].Item2;
            int i = 0;
            foreach (var item in answers.Children)
            {
                var radio = item as RadioButton;
                radio.Content = $"{i + 1}-{list[i]}";
                i++;
            }
        }
        
        public void ClearSelection()
        {
            foreach (var item in answers.Children)
            {
                var radio = item as RadioButton;
                radio.IsChecked = false;
            }
            playerResponse = "";
        }
        //Обрабатываем файлик с ответами
        static public string GetCorrectAnswers()
        {
            string ans = "";
            using (StreamReader sr = new StreamReader(@"..\..\..\Answers.txt"))
            {
                ans = sr.ReadLine();
            }
            return ans;
        }
        //Обрабатываем файлик с вопросами и заполняем лист totalQuestions
        static public void GetAllQuestions()
        {

            using (StreamReader sr = new StreamReader(@"..\..\..\Questions.txt"))
            {
                totalQuestions = int.Parse(sr.ReadLine());
                for (int i = 0; i < totalQuestions; i++)
                {
                    string ques = sr.ReadLine();
                    var list = new List<string>();
                    for (int j = 0; j < 4; j++)
                    {
                        string s = sr.ReadLine();
                        list.Add(s);
                    }
                    var tup = Tuple.Create(ques, list);
                    questions.Add(tup);
                }
            }
        }
        //При клике на кнопку рестарт вызывается данная функция
        private void restart_Click(object sender, RoutedEventArgs e)
        {
            Restart();
        }
    }
}
