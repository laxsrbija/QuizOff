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
using System.Windows.Shapes;

namespace QuizOff.Views
{
    /// <summary>
    /// Interaction logic for SignUpDialog.xaml
    /// </summary>
    public partial class SignUpDialog : Window
    {

        public User CurrentUser { private set; get; }

        public SignUpDialog()
        {
            InitializeComponent();
        }

        private void SignUp(object sender, RoutedEventArgs e)
        {
            bool result = CreateUser();

            if (result)
            {
                DialogResult = true;

            } else
            {
                Invalidate();
            }

        }

        private void Invalidate()
        {
            SignupUsername.Foreground = new SolidColorBrush(Colors.Red);
        }

        private bool CreateUser()
        {

            string username = SignupUsername.Text;
            string password = Utils.Hashing.HashPassword(username, SignupPassword.Password);

            if (username.Length == 0 || password.Length == 0)
            {
                return false;
            }

            using (var db = new DbHelper())
            {
                if (db.SelectSingleObject("select iduser from user where username = @u", new Dictionary<string, string>() { ["@u"] = username }) == null)
                {
                    var id = db.Insert("insert into user (username, password) values (@u, @p)", new Dictionary<string, string>() { ["@u"] = username, ["@p"] = password });
                    if (id > 0)
                    {
                        CurrentUser = new User(id.ToString(), username);
                        return true;
                    }
                }
            }

            return false;

        }

        private void OnChange()
        {
            SignupUsername.Foreground = new SolidColorBrush(Colors.Black);
        }

        private void SignupUsername_TextChanged(object sender, TextChangedEventArgs e)
        {
            OnChange();
        }

        private void SignupPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            OnChange();
        }

    }

}
