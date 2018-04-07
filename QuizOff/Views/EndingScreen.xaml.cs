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

        private Game game;

        public EndingScreen(Game game)
        {

            InitializeComponent();

            this.game = game;

            using (var db = new DbHelper())
            {
                TestLabel.Content = db.SelectSingleObject("select total_points from game where idgame = " + game.DbGameId).ToString();
            }

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            game.Main.MainFrame = new Category(game.Main, "2");
        }
    }
}
