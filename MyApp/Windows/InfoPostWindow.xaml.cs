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
using System.Windows.Shapes;

namespace MyApp.Windows
{
    /// <summary>
    /// Логика взаимодействия для InfoPostWindow.xaml
    /// </summary>
    public partial class InfoPostWindow : Window
    {
        SQLiteDB db;
        public InfoPostWindow(IPost post)
        {
            InitializeComponent();
            db = new SQLiteDB();
            DataContext = post;

            
            
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            int id = Convert.ToInt32(idInput.Text);
            string description = descriptionInput.Text;
            string body = bodyInput.Text;
            db.UpdatePost(id, description, body);
        }

        
    }
}
