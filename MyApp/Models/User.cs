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
        //public static User us;
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

    public class IPost
    {
        public int Id { get; set; }
        public string Description { get; set; } = "";
        public string Body { get; set; }
        public int UserId { get; set; }
    }
}
