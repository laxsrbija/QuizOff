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
    // TODO: Napraviti okvir Question, koji ce sadrzati zajednicke elemente za dva pitanja i okvir po kome se razlikuju. 
    {

        private MainWindow main;
        public Question GetQuestion{ private set; get; }

        public TextQuestion(MainWindow main, string id)
        {
            InitializeComponent();

            this.main = main;
            GetQuestion = LoadQuestion(id);
            DataContext = this;

        }

        // TODO: Ucitavanje treba da vrsi instanca klase Game i to ucitavanjem svih pitanja odjednom.
        // Ostavljeno kao privremeno resenje.
        private Question LoadQuestion(string id)
        {

            Question question = null;

            using (var db = new DbHelper())
            {

                var result = db.SelectSingleRow(
                    new string[] { "idquestion", "question_text", "has_image", "image_url", "answer_1_c", "answer_2", "answer_3", "answer_4" }, 
                    "from question where idquestion = @id", new Dictionary<string, string>() { ["@id"] = id }
                );

                string text = result["question_text"].ToString();
                string[] answers = new string[] { result["answer_1_c"].ToString(), result["answer_2"].ToString(), result["answer_3"].ToString(), result["answer_4"].ToString() };
                string image = null;
                if (Convert.ToUInt32(result["has_image"]) == 1)
                {
                    image = result["image_url"].ToString();
                }

                question = new Question(id, text, answers, image);

            }

            return question;

        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {

            var button = sender as Button;
            if (GetQuestion.CheckAnswer(button.Content.ToString()))
            {
                button.Background = new SolidColorBrush(Colors.Green);
            }

        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            main.MainFrame.Content = new Category(main, 1);
        }
    }
}
