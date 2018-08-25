using QuizOff.Models;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

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
            string username = Username.Text;
            string hashedPassword = Utils.Hashing.HashPassword(Username.Text, Password.Password);

            int id = CheckLogin(username, hashedPassword);
            if (id != -1)
            {
                main.CurrentUser = new User(id.ToString(), Username.Text);
                main.MainFrame = new MainMenu(main);
            }
            
        }

        private void SignUpButton_Click(object sender, RoutedEventArgs e)
        {

            var dialog = new SignUpDialog();

            if ((bool)dialog.ShowDialog())
            {
                main.CurrentUser = dialog.CurrentUser;
                main.MainFrame = new MainMenu(main);
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
