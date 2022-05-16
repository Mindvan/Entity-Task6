using MySQLApp;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace HelloApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
               
                string[] textInput;
                while (true) {
                    Console.WriteLine("Please type the command like this:");
                    Console.WriteLine("if insert - add [login] [password] [name] [age]");
                    Console.WriteLine("if remove - remove [login]");
                    Console.WriteLine("if display - display\n");
                    textInput = Console.ReadLine().Split();
                    if (textInput[0].ToLower() == "insert")
                    {
                        string login = textInput[1];
                        string password = textInput[2];
                        string name = textInput[3].ToString();
                        int age = Int32.Parse(textInput[4]);
                        Insert(login, password, name, age);
                    }
                    else if (textInput[0].ToLower() == "remove")
                    {
                        string login = textInput[1];
                        Remove(login);
                    }
                    else if (textInput[0].ToLower() == "display")
                    {
                        Display();
                    }
                    else {
                        Console.WriteLine("\nYour command was incorrent, please try again\n");
                    }
                }
                
            }
        }

        public static void Insert(string login, string password, string name, int age)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = new User { Login = login, Password = GetMD5(password), Name = name, Age = age };
                if (GetData(login) is null)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    Console.WriteLine("Insertion was successful.\n");
                }
                else {
                    Console.WriteLine("This login has already been added.\n");
                }
            }
        }

        public static User GetData(string login)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user_query = db.Users.Where(p => p.Login == login);
                if (user_query.Any())
                    return user_query.First();
                else
                    return null;
            }
        }

        public static void Display()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user_query = db.Users;
                foreach (User user in user_query)
                    Console.WriteLine($"OUTPUT: {user.Login} | {user.Password} | {user.Name} | {user.Age}");
                Console.WriteLine("\n");
            }
        }

        public static void Remove(string login) // удалить элемент
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = GetData(login);
                if (user is not null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                    Console.WriteLine("The removal was successful.\n");
                } else
                {
                    Console.WriteLine("The login does not exist.\n");
                }
            }
        }
        static string GetMD5(string password) // хэширование
        {
            byte[] hash = Encoding.ASCII.GetBytes(password);
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] hashenc = md5.ComputeHash(hash);
            string result = "";
            foreach (var b in hashenc)
            {
                result += b.ToString("x2");
            }
            return result;
        }
    }
}