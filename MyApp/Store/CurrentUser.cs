using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp
{
    internal class CurrentUser
    {
        public static int Id { get; set; }
        public static string Login { get; set; }
        // Другие свойства пользователя

        public static void Create(int id, string login)
        {
            Id = id;
            Login = login;
        }

        public static void Clear()
        {
            Id = 0;
            Login = null;
        }
    }
}
