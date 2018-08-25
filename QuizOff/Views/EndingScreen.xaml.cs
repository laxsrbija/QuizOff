using QuizOff.Controls;
using QuizOff.Models;
using System;
using System.Collections.Generic;
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

namespace QuizOff.Views
{
    /// <summary>
    /// Interaction logic for EndingScreen.xaml
    /// </summary>
    public partial class EndingScreen : Page
    {

        public Game CurrentGame { private set; get; } // TODO: NEKAD SE NE PRIKAZU REZULTATI, VEC ZAGLAVI NA POSLEDNJEM PITANJU
        public string Points { private set; get; }
        public string Rank { private set; get; }

        public EndingScreen(Game game)
        {

            InitializeComponent();

            this.CurrentGame = game;

            DataContext = this;

            using (var db = new DbHelper())
            {

                Points = db.SelectSingleObject("select total_points from game where idgame = " + game.DbGameId).ToString();
                DisplayScoreboard(db);

            }

            Console.WriteLine("ENDING INIT");

        }

        private void PlayAgain(object sender, RoutedEventArgs e)
        {
            new Game(CurrentGame.Main, CurrentGame.CurrentCategory);
        }

        private void GoToMain(object sender, RoutedEventArgs e)
        {
            CurrentGame.Main.MainFrame = new MainMenu(CurrentGame.Main);
        }

        private void DisplayScoreboard(DbHelper db)
        {

            var scoreboard = Utils.FetchScoreboardForCategory(db, CurrentGame.CurrentCategory.Id);
            bool currentGameShown = false;

            foreach (var res in scoreboard)
            {

                var score = res;

                if (score["idgame"].ToString().Equals(CurrentGame.DbGameId.ToString()))
                {
                    currentGameShown = true;
                }

                if (Convert.ToInt32(score["rank"]) == scoreboard.Count && !currentGameShown)
                {
                    score = new Dictionary<string, object>
                    {
                        ["username"] = CurrentGame.Main.CurrentUser.Username,
                        ["total_points"] = Points,
                        ["idgame"] = CurrentGame.DbGameId.ToString(),
                        ["rank"] = Math.Max(GetCurrentGameRank(db), scoreboard.Count)
                    };
                }

                var item = new ScoreboardItem(
                    score["rank"].ToString(), 
                    score["username"].ToString(), 
                    score["total_points"].ToString()
                );

                Scoreboard.Children.Add(item);

                if (score["idgame"].ToString().Equals(CurrentGame.DbGameId.ToString()))
                {
                    item.SetColor(Color.FromRgb(143, 188, 143));
                    Rank = Utils.GetRankOrdinalString(score["rank"].ToString());
                }

            }

        }

        private int GetCurrentGameRank(DbHelper db)
        {
            return Convert.ToInt32(db.SelectSingleObject("select count(1) from game, (select total_points from game where idgame = "
                + CurrentGame.DbGameId + ") as r where game.total_points > r.total_points").ToString());
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            CurrentGame.Main.MainFrame = new CategoryDetailsScreen(CurrentGame.Main, CurrentGame.CurrentCategory);
        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            new Game(CurrentGame.Main, CurrentGame.CurrentCategory);
        }

    }
    
}
