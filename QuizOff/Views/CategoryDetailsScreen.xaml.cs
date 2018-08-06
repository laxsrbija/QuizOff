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
    /// Interaction logic for Category.xaml
    /// </summary>
    public partial class CategoryDetailsScreen : Page
    {

        private MainWindow main;
        public Models.Category CurrentCategory { private set; get; }
        public string Username { private set; get; }

        public CategoryDetailsScreen(MainWindow main, Models.Category category)
        {

            InitializeComponent();

            this.main = main;
            this.CurrentCategory = category;
            this.Username = main.CurrentUser.Username;

            DisplayScores();

            DataContext = this;

        }

        private void StartGame(object sender, RoutedEventArgs e)
        {
            new Game(main, CurrentCategory);
        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            main.MainFrame = new CategoryListScreen(main);
        }

        private void DisplayScores()
        {

            var scoreboard = Utils.FetchScoreboardForCategory(new DbHelper(), CurrentCategory.Id, 10);

            foreach (var score in scoreboard)
            {
                Scoreboard.Children.Add(new ScoreboardItem(
                    score["rank"].ToString(), 
                    score["username"].ToString(), 
                    score["total_points"].ToString(),
                    5
                ));
            }

        }

    }
}
