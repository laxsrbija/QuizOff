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

namespace QuizOff.Controls
{
    /// <summary>
    /// Interaction logic for ScoreboardItem.xaml
    /// </summary>
    public partial class ScoreboardItem : UserControl
    {

        public ScoreboardItem(string rank, string username, string points, double margin = 10)
        {

            InitializeComponent();

            Rank.Content = rank;
            Username.Content = username;
            Points.Content = points;
            Container.Margin = new Thickness(margin);

        }

        public void SetColor(Color color)
        {
            //todo
        }

    }

}
