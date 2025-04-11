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

namespace MyApp.Pages
{
    /// <summary>
    /// Логика взаимодействия для Registration.xaml
    /// </summary>
    public partial class Registration : Page
    {
        SQLiteDB db;
        public Registration()
        {
            InitializeComponent();
            db = new SQLiteDB();
            db.CreateTableUsers();
        }

        private void Resistr(object sender, RoutedEventArgs e)
        {
            string login = Login.Text;
            string password = Password.Password;
            string password2 = Password2.Password;

            if (login.Length < 3)
            {
                MessageBox.Show("Логин должен содержать не менее 3 символов",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                Login.ToolTip = "Не менее 3 символов";
                Login.Background = Brushes.Red;
                return;
            }

            if (password != password2) {
                MessageBox.Show("Повторный пароль не совпадает",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                Password2.ToolTip = "Не совпадает с паролем";
                Password2.Background = Brushes.Red;
                return;
            }

            
            if(db.Registration(login, password))
            {
            NavigationService.Navigate(new Login());
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Login());
        }
    }
}
