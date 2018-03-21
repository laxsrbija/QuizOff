using QuizOff.Views;
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
using System.Windows.Threading;

namespace QuizOff
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        private DispatcherTimer timer;
        private LoginMenu login;

        public MainWindow()
        {

            InitializeComponent();

            MainFrame.Content = new LoginMenu(MainFrame);

            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1); // tajmer otkucava jednom u sekundi
            timer.Tick += Timer_Tick;
            
            new DbHelper();

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            login.Time.Content = (int)login.Time.Content + 1;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            //MessageBox.Show(EasyEncryption.SHA.ComputeSHA256Hash("LAX"));

            login = (LoginMenu)MainFrame.Content;
            login.Time.Content = 0;
            timer.Start();

        }
    }
}
