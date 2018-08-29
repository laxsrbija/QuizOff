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
            }

        }

        private bool CreateUser()
        {

            string username = SignupUsername.Text;
            
            if (!Utils.ValidateUsername(username))
            {
                new ErrorDialog("Invalid username").ShowDialog();
                return false;
            }

            if (SignupPassword.Password.Length == 0)
            {
                new ErrorDialog("Password cannot be empty").ShowDialog();
                return false;
            }

            string password = Utils.Hashing.HashPassword(username, SignupPassword.Password);

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
                } else
                {
                    new ErrorDialog("Username already exists").ShowDialog();
                }
            }

            return false;

        }

    }

}
