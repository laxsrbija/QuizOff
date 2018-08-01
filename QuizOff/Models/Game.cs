using QuizOff.Views;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Windows;
using Yort.Ntp;

namespace QuizOff.Models
{
    public class Game
    {

        public MainWindow Main { private set; get; } 
        public Category CurrentCategory { private set; get; }

        private List<Question> questions;
        public int QuestionNumber { private set; get; }

        public long DbGameId { private set; get; }

        public Game(MainWindow main, Category category)
        {
            Main = main;
            CurrentCategory = category;
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

            new Thread(() =>
            {

                Thread.CurrentThread.IsBackground = true;

                var client = new NtpClient(Utils.Parameters.Ntp.NTP_SERVER);
                client.TimeReceived += InsertGameStartTime;
                client.BeginRequestTime();

            }).Start();

            return list;

        }

        private void InsertGameStartTime(object sender, NtpTimeReceivedEventArgs e)
        {
            var time = Utils.DateTimeToMySqlFormat(e.CurrentTime);
            Console.WriteLine("Game start time: {0}", time);
            using (var db = new DbHelper())
            {
                DbGameId = db.Insert("insert into game (user_iduser, category_idcategory, time_start) values (" + Main.CurrentUser.Id + ", " + CurrentCategory.Id + ", '" + time + "')");
            }
        }

        public void ShowNextQuestion()
        {

            if (QuestionNumber >= Utils.Parameters.Quiz.NUMBER_OF_QUESTIONS)
            {

                new Thread(() =>
                {

                    Thread.CurrentThread.IsBackground = true;

                    var client = new NtpClient(Utils.Parameters.Ntp.NTP_SERVER);
                    client.TimeReceived += UpdateGameEndTime;
                    client.BeginRequestTime();

                }).Start();

                return;

            }

            Main.MainFrame = new QuestionTemplate(this, questions[QuestionNumber++]);

        }

        private void UpdateGameEndTime(object sender, NtpTimeReceivedEventArgs e)
        {

            var time = Utils.DateTimeToMySqlFormat(e.CurrentTime);
            Console.WriteLine("Game end time: {0}", time);

            using (var db = new DbHelper())
            {
                db.Update("update game set time_end = '" + time + "' where idgame = " + DbGameId);
            }

            Application.Current.Dispatcher.Invoke((Action)(() => {
                Main.MainFrame = new EndingScreen(this);
            }));

        }

        public void QuestionStarted(Question question)
        {
            new Thread(() =>
            {

                Thread.CurrentThread.IsBackground = true;

                var client = new NtpClient(Utils.Parameters.Ntp.NTP_SERVER);
                client.TimeReceived += (sender, e) => InsertQuestionStartTime(sender, e, question);
                client.BeginRequestTime();

            }).Start();
        }

        private void InsertQuestionStartTime(object sender, NtpTimeReceivedEventArgs e, Question question)
        {
            var time = Utils.DateTimeToMySqlFormat(e.CurrentTime);
            Console.WriteLine("Question {1} for game {2} start time: {0}", time, QuestionNumber, DbGameId);
            using (var db = new DbHelper())
            {
                question.DbGameQuestionId = db.Insert("insert into game_questions (game_idgame, question_idquestion, time_served) values ("
                    + DbGameId.ToString() + ", " + question.Id + ", '" + time + "')");
            }
        }

        public void QuestionAnswered(long dbGameQuestionId, string status)
        {
            new Thread(() =>
            {

                Thread.CurrentThread.IsBackground = true;

                var client = new NtpClient(Utils.Parameters.Ntp.NTP_SERVER);
                client.TimeReceived += (sender, e) => UpdateQuestionEndTime(sender, e, dbGameQuestionId, status);
                client.BeginRequestTime();

            }).Start();
        }

        private void UpdateQuestionEndTime(object sender, NtpTimeReceivedEventArgs e, long dbGameQuestionId, string status)
        {
            var time = Utils.DateTimeToMySqlFormat(e.CurrentTime);
            Console.WriteLine("Question {1} end time: {0}", time, QuestionNumber);
            using (var db = new DbHelper())
            {
                db.Update("update game_questions set time_completed = '" + time + "', question_status = '" + status + "' where idgame_questions = " + dbGameQuestionId);
            }
        }

        // TODO: Dodati #region sekcije radi citljivosti

    }

}
