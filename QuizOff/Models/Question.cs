using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizOff.Models
{
    public class Question
    {

        public Question(string id, string text, string[] answers, string imageUrl, Random random)
        {

            Id = id;
            Text = text;
            ImageUrl = imageUrl;

            correctAnswer = answers[0];

            this.answers = new List<string>(answers);
            this.answers.Shuffle(random);

        }

        private string correctAnswer;

        public string Id { private set; get; }
        public string Text { private set; get; }
        public string ImageUrl { private set; get; }

        public long DbGameQuestionId { set; get; }

        private List<string> answers;
        public string this[int id] {
            get {
                if (id >= 0 && id < 4)
                {
                    return answers[id];
                }
                return null;
            }
        }

        public bool CheckAnswer(string answer)
        {
            return correctAnswer.Equals(answer);
        }

    }
}
