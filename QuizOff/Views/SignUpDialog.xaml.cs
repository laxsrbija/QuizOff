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
        
        // TODO: Omogućiti dugme tek kada oba polja budu popunjena

        public SignUpDialog()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = true;
        }

        public string Username {
            get => SignupUsername.Text;
        }

        public string Password {
            get => SignupPassword.Password;
        }

    }

}
