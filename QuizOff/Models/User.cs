using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizOff.Models
{
    public class User
    {

        public string Id { set; get; }
        public string Username { set; get; }

        public User(string id, string username)
        {
            this.Id = id;
            this.Username = username;
        }

        public override string ToString()
        {
            return "User with ID " + Id + " and username " + Username;
        }

    }
}
