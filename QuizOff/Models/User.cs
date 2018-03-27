using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizOff.Models
{
    public class User
    {

        private string id;
        private string username;
        private string hashedPassword;

        public User(string id, string username, string hashedPassword)
        {
            this.id = id;
            this.username = username;
            this.hashedPassword = hashedPassword;
        }

        public string Id {
            get => id;
        }

        public string Username {
            get => username;
        }

        public string HashedPassword {
            get => hashedPassword;
        }

        public override string ToString()
        {
            return "User with ID " + id + " and username " + username;
        }

    }
}
