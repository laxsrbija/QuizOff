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

namespace QuizOff.Controls
{
    /// <summary>
    /// Interaction logic for CategoryItem.xaml
    /// </summary>
    public partial class CategoryItem : UserControl
    {

        private MainWindow main;
        private QuizOff.Models.Category category;

        public CategoryItem(MainWindow main, QuizOff.Models.Category category)
        {

            InitializeComponent();

            this.category = category;
            this.main = main;

            Picture.ImageSource = new BitmapImage(new Uri(category.ImageUrl, UriKind.Absolute));
            Title.Content = category.Name;

        }

        private void SelectCategory(object sender, MouseButtonEventArgs e)
        {
            main.MainFrame = new QuizOff.Views.Category(main, category);
        }

    }
}
