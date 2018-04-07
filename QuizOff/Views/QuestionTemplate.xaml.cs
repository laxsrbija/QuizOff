using QuizOff.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
using System.Windows.Threading;

namespace QuizOff.Views
{
    /// <summary>
    /// Interaction logic for QuestionTemplate.xaml
    /// </summary> 
    // TODO Refaktorisati ceo dokument
    public partial class QuestionTemplate : Page, INotifyPropertyChanged
    {

        public Game CurrentGame { private set; get; }
        public Question CurrentQuestion { private set; get; }
        public Page QuestionTextPage { private set; get; }

        private bool gameRunning;
        private Button[] buttons;

        private DispatcherTimer questionTimer;
        public int questionTime;
        public int Time 
        { 
            private set {
                questionTime = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Time"));
            }
            get => questionTime;
        }

        private DispatcherTimer displayAnswersTimer;
        private int displayAnswersTime;

        private DispatcherTimer displayCorrectAnswerTimer;
        private int displayCorrectAnswerTime;

        public event PropertyChangedEventHandler PropertyChanged;

        public QuestionTemplate(Game game, Question question)
        {

            InitializeComponent();

            CurrentGame = game;
            CurrentQuestion = question;

            buttons = new Button[] { B1, B2, B3, B4 };
            gameRunning = false;

            foreach (var button in buttons) // TODO Sakriti U XAML-u nakon zavrsetka dizajna
            {
                button.Visibility = Visibility.Hidden;
            }

            Time = Utils.Parameters.Quiz.TIME_TO_ANSWER;
            questionTimer = new DispatcherTimer();
            questionTimer.Interval = TimeSpan.FromSeconds(1);
            questionTimer.Tick += QuestionTimer_Tick;

            displayAnswersTime = 0;
            displayAnswersTimer = new DispatcherTimer();
            displayAnswersTimer.Interval = TimeSpan.FromSeconds(1);
            displayAnswersTimer.Tick += DisplayAnswersTimer_Tick;

            displayCorrectAnswerTime = 0;
            displayCorrectAnswerTimer = new DispatcherTimer();
            displayCorrectAnswerTimer.Interval = TimeSpan.FromSeconds(1);
            displayCorrectAnswerTimer.Tick += DisplayCorrectAnswerTimer_Tick;

            if (CurrentQuestion.ImageUrl != null)
            {
                QuestionTextPage = new PictureQuestion(this);
            } else
            {
                QuestionTextPage = new TextQuestion(this);
            }

            DataContext = this;

        }

        private void DisplayAnswersTimer_Tick(object sender, EventArgs e)
        {
            if (++displayAnswersTime >= Utils.Parameters.Quiz.TIME_TO_READ)
            {

                foreach (var button in buttons)
                {
                    button.Visibility = Visibility.Visible;
                }

                gameRunning = true;
                questionTimer.Start();
                displayAnswersTimer.Stop();

            }
        }

        private void QuestionTimer_Tick(object sender, EventArgs e)
        {

            if (!gameRunning || --Time == 0)
            {
                questionTimer.Stop();
            }

            if (Time == 0)
            {
                QuestionFinished();
            }

        }

        private void DisplayCorrectAnswerTimer_Tick(object sender, EventArgs e)
        {
            if (++displayCorrectAnswerTime >= Utils.Parameters.Quiz.TIME_TO_DISPLAY_ANSWER)
            {
                displayCorrectAnswerTimer.Stop();
                CurrentGame.ShowNextQuestion();
            }
        }

        private void AnswerButton_Click(object sender, RoutedEventArgs e)
        {

            if (gameRunning)
            {
                QuestionFinished(sender as Button);
            }

        }

        private void QuestionFinished(Button button = null)
        {

            gameRunning = false;

            if (button != null)
            {
                button.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(Utils.Parameters.Quiz.COLOR_WRONG_ANSWER));
            }
            
            var status = VerifyAnswer(button);

            //Console.WriteLine(status.ToString());

            displayCorrectAnswerTimer.Start();
            CurrentGame.QuestionAnswered(CurrentQuestion.DbGameQuestionId, status.ToString());

        }

        private enum QuestionStatus { UNANSWERED, CORRECT, INCORRECT };

        /// <summary>
        /// Verifies the validity of the answer (if chosen) and marks the correct answer
        /// </summary>
        /// <param name="b">Button representing the chosen answer</param>
        /// <returns>QuestionStatus enum containing the question's state</returns>
        private QuestionStatus VerifyAnswer(Button b)
        {

            foreach (var button in buttons)
            {
                if (CurrentQuestion.CheckAnswer(button.Content.ToString()))
                {

                    if (b == null)
                    {
                        button.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(Utils.Parameters.Quiz.COLOR_UNANSWERED));
                        return QuestionStatus.UNANSWERED;
                    }

                    button.Background = (SolidColorBrush)(new BrushConverter().ConvertFrom(Utils.Parameters.Quiz.COLOR_CORRECT_ANSWER));
                    if (button.Equals(b))
                    {
                        return QuestionStatus.CORRECT;
                    }

                    break;

                }
            }

            return QuestionStatus.INCORRECT;

        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {

            CurrentGame.QuestionLoaded(CurrentQuestion);
            displayAnswersTimer.Start();

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            questionTimer.Stop();
            displayAnswersTimer.Stop();
            displayCorrectAnswerTimer.Stop();
            CurrentGame.Main.MainFrame = new Category(CurrentGame.Main, CurrentGame.CurrentCategory.Id);
        }

    }
}
