using QuizOff.Models;
using QuizOff.Views;
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
using System.Windows.Threading;

namespace QuizOff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {

        public event PropertyChangedEventHandler PropertyChanged;

        private Page mainFrame;
        public Page MainFrame {
            get => mainFrame;
            set {
                mainFrame = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("MainFrame"));
            }
        }

        private User user;
        public User CurrentUser {
            get => user;
            set {
                user = value;
                if (CurrentUser == null)
                {
                    MainFrame = new LoginMenu(this);
                }
            }
        }

        public MainWindow()
        {

            InitializeComponent();

            CurrentUser = null;

            DataContext = this;

        }

    }
}
