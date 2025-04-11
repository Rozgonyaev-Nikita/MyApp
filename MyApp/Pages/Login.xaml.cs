using MyApp.Windows;
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
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Page
    {
        SQLiteDB db;
        public Login()
        {
            InitializeComponent();
            db = new SQLiteDB();
        }

        private void Enter(object sender, RoutedEventArgs e)
        {
            string login = LoginInput.Text;
            string password = PasswordInput.Password;
            if (db.Avtoriz(login, password))
            {
                //MessageBox.Show("Вход успешен!");
                SecondWindow second = new SecondWindow();
                second.Show();
                Window.GetWindow(this).Hide();
            }
            else
            {
                MessageBox.Show("Данные не верны!");
            }
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Registration());
        }
    }
}
