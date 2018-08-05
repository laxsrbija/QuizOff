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

        private Game game;// TODO: NEKAD SE NE PRIKAZU REZULTATI, VEC ZAGLAVI NA POSLEDNJEM PITANJU

        public string Points { private set; get; }

        public EndingScreen(Game game)
        {

            InitializeComponent();

            this.game = game;
            DataContext = this;

            using (var db = new DbHelper())
            {

                Points = db.SelectSingleObject("select total_points from game where idgame = " + game.DbGameId).ToString();
                DisplayScoreboard(db);

            }

        }

        private void PlayAgain(object sender, RoutedEventArgs e)
        {
            //new Game(game.Main, new Models.Category(game.CurrentCategory.Id));
        }

        private void ReturnToMain(object sender, RoutedEventArgs e)
        {
            game.Main.MainFrame = new MainMenu(game.Main);
        }

        private void DisplayScoreboard(DbHelper db)
        {

            var scoreboard = Utils.FetchScoreboardForCategory(db, game.CurrentCategory.Id);

            bool currentGameShown = false;

            foreach (var res in scoreboard)
            {

                var score = res;

                if (score["idgame"].ToString().Equals(game.DbGameId.ToString()))
                {
                    currentGameShown = true;
                }

                if (Convert.ToInt32(score["rank"]) == scoreboard.Count && !currentGameShown)
                {
                    score = new Dictionary<string, object>
                    {
                        ["username"] = game.Main.CurrentUser.Username,
                        ["total_points"] = Points,
                        ["idgame"] = game.DbGameId.ToString(),
                        ["rank"] = GetCurrentGameRank(db)
                    };
                }

                StackPanel panel = new StackPanel();
                panel.Orientation = Orientation.Horizontal;

                Label l1 = new Label();
                l1.Content = score["rank"];
                panel.Children.Add(l1);

                Label l2 = new Label();
                l2.Content = score["username"];
                panel.Children.Add(l2);

                Label l3 = new Label();
                l3.Content = score["total_points"];
                panel.Children.Add(l3);

                Scoreboard.Children.Add(panel);

                if (score["idgame"].ToString().Equals(game.DbGameId.ToString()))
                { 

                    var currentColor = new SolidColorBrush(Color.FromRgb(143, 188, 143));

                    l1.Foreground = currentColor;
                    l2.Foreground = currentColor;
                    l3.Foreground = currentColor;

                }

            }

        }

        private int GetCurrentGameRank(DbHelper db)
        {
            return Convert.ToInt32(db.SelectSingleObject("select count(1) from game, (select total_points from game where idgame = "
                + game.DbGameId + ") as r where game.total_points > r.total_points").ToString());
        }

    }
    
}
