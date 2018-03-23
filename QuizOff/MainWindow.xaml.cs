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

            //new DbHelper();
            var helper = new DbHelper();
            var result = helper.SelectMultipleRows(new string[] { "idtest", "testcol" }, "from test");
            helper.Update("update test set testcol = 'lax' where idtest = 3");
            string idPrvog = helper.SelectSingleObject("select testcol from test where idtest = @id", new Dictionary<string, string> { ["@id"] = "3" }).ToString();
            helper.Close();

            Console.WriteLine("TEST");
            for (var i = 0; i < result.Count; i++)
            {
                var dict = result[i];
                Console.WriteLine(dict["idtest"] + " " + dict["testcol"]);
            }
            Console.WriteLine("TEST END {0}", idPrvog);

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
