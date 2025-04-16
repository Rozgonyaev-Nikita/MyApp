using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Windows;
using System.Windows.Shapes;
using System.Security.Cryptography;
using MyApp.Pages;

namespace MyApp

{
    //internal class IUser
    //{
    //    public int Id { get; set; }
    //    public string Name { get; set; }
    //    public string Password { get; set; }

    //}
    
    internal class SQLiteDB
    {

        private string path = "MyDB.sqlite";

        private SQLiteConnection HelperSQL()
        {
            return new SQLiteConnection($"Data Source={path};Version=3;");
        }
        internal void CreateTableUsers()
        {
            if (!File.Exists(path))
            {
                SQLiteConnection.CreateFile(path);

            using (SQLiteConnection conn = HelperSQL())
            {
                conn.Open();

                string sql = @"CREATE TABLE IF NOT EXISTS Users (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Login VARCHAR(100) NOT NULL UNIQUE,
                                Password VARCHAR(100) NOT NULL);

                             CREATE TABLE IF NOT EXISTS Posts (
                                Id INTEGER PRIMARY KEY AUTOINCREMENT,
                                Description VARCHAR(150) NOT NULL UNIQUE,
                                Body TEXT NOT NULL,
                                UserId INT,
                                CONSTRAINT FK_Post_User FOREIGN KEY (UserId) 
                                REFERENCES Users(Id)  
                                ON DELETE CASCADE
                                ON UPDATE CASCADE);";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.ExecuteNonQuery();
                }
            }
            }
        }

        internal bool Registration(string login, string password)
        {
            try
            {
                string hashedPassword = HashPassword(password);
                using (SQLiteConnection conn = HelperSQL())
                {
                    conn.Open();
                    string sql = "INSERT INTO Users (Login, Password) VALUES (@Login, @Password)";
                    using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                    {
                        cmd.Parameters.AddWithValue("@Login", login);
                        cmd.Parameters.AddWithValue("@Password", hashedPassword);
                        cmd.ExecuteNonQuery();
                        return true; // Успешная регистрация
                    }
                }
            }
            catch (SQLiteException ex) when (ex.ResultCode == SQLiteErrorCode.Constraint)
            {
                MessageBox.Show("Пользователь с таким логином уже существует",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Warning);
                return false;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при регистрации: {ex.Message}",
                              "Ошибка",
                              MessageBoxButton.OK,
                              MessageBoxImage.Error);
                return false;
            }
        }

        internal bool Avtoriz(string login, string password)
        {
            string hashedInputPassword = HashPassword(password);

            using (SQLiteConnection conn = HelperSQL())
            {
                conn.Open();
                string sql = "SELECT * FROM Users WHERE Login = @Login AND Password = @Password";

                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Login", login);
                    cmd.Parameters.AddWithValue("@Password", hashedInputPassword);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            // Создаем и возвращаем объект User
                            //User user = new User
                            //{
                            //    Id = Convert.ToInt32(reader["Id"]),
                            //    Login = reader["Login"].ToString(),
                            //    Password = reader["Password"].ToString()
                            //    // Добавьте другие свойства по необходимости
                            //};
                            CurrentUser.Create(Convert.ToInt32(reader["Id"]), reader["Login"].ToString());

                            return true;
                        }
                    }
                }
            }

            // Если пользователь не найден
            return false;
        }

        internal void CreatePost(string description, string body, int userId)
        {
            using (SQLiteConnection conn = HelperSQL())
            {
                conn.Open();
                string sql = "INSERT INTO Posts (Description, Body, UserId) VALUES (@Description, @Body, @UserId)";
                using(SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Body", body);
                    cmd.Parameters.AddWithValue("@UserId", userId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal List<IPost> GetPosts(int id)
        {
            using (SQLiteConnection conn = HelperSQL())
            {
                conn.Open();
                string sql = "SELECT * FROM Posts WHERE UserId = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);

                    using (SQLiteDataReader reader = cmd.ExecuteReader())
                    {
                        List<IPost> posts = new List<IPost>();
                        while (reader.Read()) 
                        {
                            posts.Add(new IPost
                            {
                                Id = Convert.ToInt32(reader["Id"]),
                                Description = Convert.ToString(reader["Description"]),
                                Body = Convert.ToString(reader["body"]),
                                UserId = Convert.ToInt32(reader["userId"])
                            });
                        }
                        return posts;
                    }
                }
            }
        }

        internal void DeletePost(int id)
        {
            using (SQLiteConnection conn = HelperSQL())
            {
                conn.Open();
                string sql = "DELETE FROM Posts WHERE Id = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        internal void UpdatePost(int id, string description, string body)
        {
            using(SQLiteConnection conn = HelperSQL())
            {
                conn.Open();
                string sql = "UPDATE Posts SET Description=@Description, Body = @Body WHERE Id = @Id";
                using (SQLiteCommand cmd = new SQLiteCommand(sql, conn))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@Description", description);
                    cmd.Parameters.AddWithValue("@Body", body);
                    cmd.ExecuteNonQuery();
                }
            }
        }
        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] salt = Encoding.UTF8.GetBytes("your_salt_here");
                byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

                byte[] combined = new byte[salt.Length + passwordBytes.Length];
                Buffer.BlockCopy(salt, 0, combined, 0, salt.Length);
                Buffer.BlockCopy(passwordBytes, 0, combined, salt.Length, passwordBytes.Length);

                byte[] hash = sha256.ComputeHash(combined);
                return Convert.ToBase64String(hash);
            }
        }
    }
}
