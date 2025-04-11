using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    internal class User
    {
    //User usd = new User() { };
        public static User us;
        public int Id { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }

        public User() { }
        public User(int id, string login, string password)
        {
            Id = id;
            Login = login;
            Password = password;
        }

    }
}
