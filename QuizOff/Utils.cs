using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace QuizOff
{
    public static class Utils
    {

        public static class Parameters
        {

            public static class Db
            {

                public const string DB_HOST = "piro.ddns.net";
                public const string DB_PORT = "3306";
                public const string DB_USER = "quizoff";
                public const string DB_PASSWORD = "Gw3mkRRQGZTx14co";
                public const string DB_NAME = "quizoff";

            }

            public static class Quiz
            {

                public const int NUMBER_OF_QUESTIONS = 10;
                public const int TIME_TO_ANSWER = 5;
                public const int TIME_TO_READ = 2;
                public const int TIME_TO_DISPLAY_ANSWER = 2;

                public const string COLOR_CORRECT_ANSWER = "#8fbc8f";
                public const string COLOR_WRONG_ANSWER = "#f2115b";
                public const string COLOR_UNANSWERED = "#e1ca09";

            }

            public static class Ntp
            {

                // rs.pool.ntp.org
                public const string NTP_SERVER = "147.91.8.77";

            }

        }

        public static class Hashing
        {

            private const string SALT_LEFT = "2+h5~Um9bN}PUP4%";
            private const string SALT_RIGHT = "Cr3m]*UC/};8au3.";

            public static string HashPassword(string username, string unhashedPassword)
            {
                return EasyEncryption.SHA.ComputeSHA256Hash(username + SALT_LEFT + unhashedPassword + SALT_RIGHT);
            }

        }

        // Fisher-Yates Shuffle
        public static void Shuffle<T>(this List<T> list, Random random) //
        {
            for (var i = 0; i < list.Count; i++)
            {
                list.Swap(i, random.Next(i, list.Count));
            }
        }

        public static void Swap<T>(this List<T> list, int i, int j)
        {
            var tmp = list[i];
            list[i] = list[j];
            list[j] = tmp;
        }

        public static string DateTimeToMySqlFormat(DateTime date)
        {
            return date.ToString("yyyy-MM-dd HH:mm:ss");
        }

        public static void ToggleAudio(MainWindow main)
        {

            Properties.Settings.Default.PlayAudio = !Properties.Settings.Default.PlayAudio;
            Properties.Settings.Default.Save();

            UpdateAudioIcon(main);

        }

        public static void UpdateAudioIcon(MainWindow main)
        {
            main.AudioIcon = Properties.Settings.Default.PlayAudio ? "on" : "off";
        }

        public static List<Dictionary<string, object>> FetchScoreboardForCategory(DbHelper db, string categoryId, int numberOfRows = 5)
        {

            var scores = db.SelectMultipleRows(
                new string[] { "idgame", "total_points", "username" },
                "from game, user where iduser = user_iduser and category_idcategory = @id order by total_points desc limit " + numberOfRows,
                new Dictionary<string, string>() { ["@id"] = categoryId }
            );

            for (var i = 0; i < scores.Count; i++)
            {
                scores[i]["rank"] = i + 1;
            }

            return scores;

        }

        public static string GetRankOrdinalString(string rank)
        {

            string sufix;

            if (rank.Equals("11") || rank.Equals("12") || rank.Equals("13"))
            {
                sufix = "th";
            } else
            {

                var last = rank.Last();

                switch (last)
                {
                    case '1':
                        sufix = "st";
                        break;
                    case '2':
                        sufix = "nd";
                        break;
                    case '3':
                        sufix = "rd";
                        break;
                    default:
                        sufix = "th";
                        break;
                }

            }

            return rank + sufix;

        }

        public static bool ValidateUsername(string username)
        {

            if (username == null || username.Length < 3)
            {
                return false;
            }

            var regex = new Regex("^[a-zA-Z0-9]+$");
            return regex.IsMatch(username);

        }

    }
}
