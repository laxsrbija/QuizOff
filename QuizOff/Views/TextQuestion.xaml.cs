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
    /// Interaction logic for TextQuestion.xaml
    /// </summary>
    public partial class TextQuestion : Page
    {

        public QuestionTemplate ParentTemplate { private set; get; }

        public TextQuestion(QuestionTemplate template)
        {
            InitializeComponent();
            ParentTemplate = template;
            DataContext = this;
        }

    }
}
