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

namespace QuizOff.Views
{
    /// <summary>
    /// Interaction logic for MainMenu.xaml
    /// </summary>
    public partial class MainMenu : Page
    {

        public MainWindow Main { private set; get; }

        public MainMenu(MainWindow main)
        {

            InitializeComponent();

            this.Main = main;

            DataContext = Main;

        }

        private void LogOffButton_Click(object sender, RoutedEventArgs e)
        {
            Main.CurrentUser = null;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            Main.MainFrame = new CategoryScreen(Main);
        }

        private void ToggleAudio(object sender, RoutedEventArgs e)
        {
            Utils.ToggleAudio(Main);
        }

    }
}
