using QuizOff.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizOff.Models
{
    public class Game
    {

        public MainWindow Main { private set; get; } 
        public Category CurrentCategory { private set; get; }

        private List<Question> questions;
        public int QuestionNumber { private set; get; }

        public Game(MainWindow main, Category category)
        {
            Main = main;
            CurrentCategory = category;
            Points = 0;
            questions = LoadQuestions();
            QuestionNumber = 0;
            ShowNextQuestion();
        }

        private List<Question> LoadQuestions()
        {

            var list = new List<Question>();
            var random = new Random();

            using (var db = new DbHelper())
            {

                var result = db.SelectMultipleRows(
                    new string[] { "idquestion", "question_text", "has_image", "image_url", "answer_1_c", "answer_2", "answer_3", "answer_4" },
                    "from question where category_idcategory = @id order by rand() limit " + Utils.Parameters.Quiz.NUMBER_OF_QUESTIONS, 
                    new Dictionary<string, string>() { ["@id"] = CurrentCategory.Id }
                );

                foreach (var q in result)
                {

                    string id = q["idquestion"].ToString();
                    string text = q["question_text"].ToString();
                    string image = Convert.ToInt32(q["has_image"]) > 0 ? q["image_url"].ToString() : null;
                    string[] answers = { q["answer_1_c"].ToString(), q["answer_2"].ToString(), q["answer_3"].ToString(), q["answer_4"].ToString() };

                    var question = new Question(id, text, answers, image, random);
                    list.Add(question);

                }

            }

            return list;

        }

        public void ShowNextQuestion()
        {

            if (QuestionNumber >= Utils.Parameters.Quiz.NUMBER_OF_QUESTIONS)
            {
                return;
            }

            Main.MainFrame = new QuestionTemplate(this, questions[QuestionNumber++]);

        }

        public int Points { private set; get; }

        public void QuestionAnswered(QuestionTemplate template)
        {
            Points += template.Points;
        }

    }
}
