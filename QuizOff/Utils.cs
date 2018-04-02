using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizOff
{
    public static class Utils
    {

        public static class Parameters
        {

            public static class Db
            {

                public const string DB_HOST = "localhost";
                public const string DB_PORT = "3306";
                public const string DB_USER = "root";
                public const string DB_PASSWORD = "";
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

    }
}
