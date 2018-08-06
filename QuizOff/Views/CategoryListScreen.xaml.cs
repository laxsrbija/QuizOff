using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace QuizOff.Views
{
    /// <summary>
    /// Interaction logic for CategoryScreen.xaml
    /// </summary>
    public partial class CategoryListScreen : Page
    {

        private MainWindow main;

        public CategoryListScreen(MainWindow main)
        {

            InitializeComponent();
            this.main = main;

            DataContext = main.CurrentUser;

            LoadAndDisplayCategories();

        }

        private void GoBack(object sender, RoutedEventArgs e)
        {
            main.MainFrame = new MainMenu(main);
        }

        private void LoadAndDisplayCategories()
        {

            using (var db = new DbHelper())
            {

                var results = db.SelectMultipleRows(
                    new string[] { "idcategory", "name", "description", "image_url" },
                    "from category where show_category = 1"
                );

                StackPanel tempStackPanel = null;

                foreach (var result in results)
                {

                    string id = result["idcategory"].ToString();
                    string name = result["name"].ToString();
                    string description = result["description"].ToString();
                    string imageUrl = result["image_url"].ToString();

                    if (tempStackPanel == null || tempStackPanel.Children.Count == 3)
                    {

                        tempStackPanel = new StackPanel
                        {
                            Orientation = Orientation.Horizontal,
                            HorizontalAlignment = HorizontalAlignment.Center
                        };

                        Categories.Children.Add(tempStackPanel);

                    }

                    var category = new Models.Category(id, name, description, imageUrl);
                    tempStackPanel.Children.Add(new Controls.CategoryItem(main, category));

                }
                
            }

        }

    }

}
