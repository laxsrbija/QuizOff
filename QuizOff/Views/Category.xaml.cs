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
    public partial class Category : Page
    {

        private MainWindow main;
        private Models.Category category;

        public Category(MainWindow main, string id)
        {
            InitializeComponent();
            this.main = main;
            category = new Models.Category(id);
            DataContext = category;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            new Game(main, category);
        }
    }
}
