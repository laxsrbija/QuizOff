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
    /// Interaction logic for CategoryScreen.xaml
    /// </summary>
    public partial class CategoryScreen : Page
    {

        private MainWindow main;

        public CategoryScreen(MainWindow main)
        {
            InitializeComponent();
            this.main = main;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            main.MainFrame.Content = new MainMenu(main);
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            main.MainFrame.Content = new Category(main, "1");
        }
    }
}
