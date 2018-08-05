using QuizOff.Models;
using System;
using System.ComponentModel;
using System.Media;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
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

                CurrentGame.QuestionStarted(CurrentQuestion);

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

            var sound = Properties.Resources.unanswered;
            if (status == QuestionStatus.CORRECT)
            {
                sound = Properties.Resources.correct;
            } else if (status == QuestionStatus.INCORRECT)
            {
                sound = Properties.Resources.incorrect;
            }

            var soundPlayer = new SoundPlayer(sound);
            if (Properties.Settings.Default.PlayAudio)
            {
                soundPlayer.Play();
            }

            displayCorrectAnswerTimer.Start();
            CurrentGame.QuestionAnswered(CurrentQuestion.DbGameQuestionId, status.ToString());

        }

        private enum QuestionStatus { UNANSWERED, CORRECT, INCORRECT };

        /// <summary>
        /// Verifies the validity of the given answer (if available) and marks the correct answer
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
            displayAnswersTimer.Start();
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            questionTimer.Stop();
            displayAnswersTimer.Stop();
            displayCorrectAnswerTimer.Stop();
            CurrentGame.Main.MainFrame = new Category(CurrentGame.Main, CurrentGame.CurrentCategory.Id);
        }

        private void ToggleAudio(object sender, RoutedEventArgs e)
        {
            Utils.ToggleAudio(CurrentGame.Main);
        }

    }
}
