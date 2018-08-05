using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizOff.Models
{

    public class Category
    {

        public string Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }

        public Category(string id, string name, string description, string imageUrl)
        {

            Id = id;
            Name = name;
            Description = description;
            ImageUrl = imageUrl;

        }

    }
}
