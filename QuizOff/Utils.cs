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

    }
}
