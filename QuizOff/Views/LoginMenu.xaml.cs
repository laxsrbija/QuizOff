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
    /// Interaction logic for LoginMenu.xaml
    /// </summary>
    public partial class LoginMenu : Page
    {

        private MainWindow main;

        public LoginMenu(MainWindow main)
        {
            InitializeComponent();

            this.main = main;
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {

            Console.WriteLine(Utils.Hashing.HashPassword(Username.Text, Password.Password));

            string username = Username.Text;
            string hashedPassword = Utils.Hashing.HashPassword(Username.Text, Password.Password);

            int id = CheckLogin(username, hashedPassword);
            if (id != -1)
            {
                main.CurrentUser = new User(id.ToString(), Username.Text, "asd");
                Console.WriteLine(main.CurrentUser);
            }

            //main.MainFrame.Content = new MainMenu(main);
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {
            if ((bool)new SignUpDialog().ShowDialog())
            {
                MessageBox.Show("Y");
            } else
            {
                MessageBox.Show("N");
            }
        }

        private int CheckLogin(string username, string hashedPassword)
        {

            int ret = -1;

            using (var db = new DbHelper())
            {
                var result = db.SelectSingleObject("select iduser from user where username = @u and password = @p", new Dictionary<string, string> { ["@u"] = Username.Text, ["@p"] = hashedPassword });
                if (result != null)
                {
                    ret = Convert.ToInt32(result);
                }
            }

            return ret;

        }

    }
}
