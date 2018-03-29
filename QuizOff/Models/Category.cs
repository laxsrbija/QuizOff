using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace QuizOff.Models
{

    class Category
    {

        public Category(int id)
        {
            using (var db = new DbHelper())
            {

                var result = db.SelectSingleRow(
                    new string[] { "name", "description", "image_url" }, 
                    "from category where idcategory = @id", 
                    new Dictionary<string, string>() { ["@id"] = id.ToString() }
                );

                Id = id;
                Name = result["name"].ToString();
                Description = result["description"].ToString();
                ImageUrl = result["image_url"].ToString();

            }
        }

        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public string ImageUrl { get; private set; }

    }
}
