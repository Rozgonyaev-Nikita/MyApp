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
    /// Логика взаимодействия для SecondWindow.xaml
    /// </summary>
    public partial class SecondWindow : Window
    {
        SQLiteDB db;

        string sort = "По возрастанию";
        public SecondWindow()
        {
            InitializeComponent();
            db = new SQLiteDB();
            id.Text = CurrentUser.Id.ToString();
            login.Text = CurrentUser.Login;
            ShowPosts();
        }

        private void CreatePostButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string description = DescriptionInput.Text;
                string body = BodyInput.Text;
                int id = CurrentUser.Id;
                db.CreatePost(description, body, id);
                MessageBox.Show("Пост добавлен");
                DescriptionInput.Text = "";
                BodyInput.Text = "";
                ShowPosts();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void ShowPosts()
        {
            var posts = db.GetPosts(CurrentUser.Id);
            var filterPosts = posts.Where(post => post.Description.Contains(filterInput.Text)).ToList();
            if (sort == "По возрастанию") 
            {
                filterPosts.Sort((a, b) => a.Id - b.Id);
            } else if(sort == "По убыванию")
            {
                filterPosts.Sort((a, b) => b.Id - a.Id);
            }
            else
            {
                MessageBox.Show("С сортировкой не так!");
            }

                postsView.ItemsSource = filterPosts;
        }

        private void UpdatePost_Click(object sender, RoutedEventArgs e)
        {
            // Получаем данные выбранного элемента
            var border = (Button)sender;
            var post = (IPost)border.DataContext; // Ваш класс Post

            // Создаем новую страницу
            var detailPage = new InfoPostWindow(post);

            // Вариант 1: Показываем как диалоговое окно
            detailPage.Show();
            //Hide();

            // Вариант 2: Используем Frame для навигации (если есть Frame в главном окне)
            // mainFrame.Navigate(detailPage);
        }

        private void Delete_Click(object sender, RoutedEventArgs e)
        {
            Button button = (Button)sender;
            int value = (int)button.Tag;

            db.DeletePost(value);
            ShowPosts();
        }

        private void RadioButton_Checked(object sender, RoutedEventArgs e)
        {

            if (db != null)
            {
                
                //MessageBox.Show("karp");
                if (sender is RadioButton radioButton && radioButton.IsChecked == true)
                {
                    sort = radioButton.Content.ToString();
                    //MessageBox.Show(sort);
                    ShowPosts();
                }
            }
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            MvvmWindow mvvm = new MvvmWindow();
            mvvm.Show();
        }

        private void filterInput_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (db != null)
            {
                ShowPosts();
            }
        }
    }
}
