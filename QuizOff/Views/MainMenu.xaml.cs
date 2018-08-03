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
    public partial class MainMenu : Page, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private MainWindow main;

        private string audioIcon;
        public string AudioIcon {
            get => audioIcon;
            set {
                audioIcon = "/QuizOff;component/Resources/volume-" + value + ".png";
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("AudioIcon"));
            }
        }

        public string Username { get; set; }

        public MainMenu(MainWindow main)
        {

            InitializeComponent();

            this.main = main;
            AudioIcon = Properties.Settings.Default.PlayAudio ? "on" : "off";
            Username = main.CurrentUser.Username;

            DataContext = this;

        }

        private void LogOffButton_Click(object sender, RoutedEventArgs e)
        {
            main.CurrentUser = null;
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            main.MainFrame = new CategoryScreen(main);
        }

        private void ToggleAudio(object sender, RoutedEventArgs e)
        {

            Properties.Settings.Default.PlayAudio = !Properties.Settings.Default.PlayAudio;
            Properties.Settings.Default.Save();

            AudioIcon = Properties.Settings.Default.PlayAudio ? "on" : "off";

        }
    }
}
