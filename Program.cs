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
               
                string[] parsed_command;
                while (true) {
                    Console.WriteLine("Please type the command like this:");
                    Console.WriteLine("if add - add [login] [password] [name] [age]");
                    Console.WriteLine("if remove - remove [login]");
                    Console.WriteLine("if display - display\n");
                    parsed_command = Console.ReadLine().Split();
                    if (parsed_command[0] == "add")
                    {
                        string login = parsed_command[1];
                        string password = parsed_command[2];
                        string name = parsed_command[3].ToString();
                        int age = Int32.Parse(parsed_command[4]);
                        addUser(login, password, name, age);
                    }
                    else if (parsed_command[0] == "remove")
                    {
                        string login = parsed_command[1];
                        removeUser(login);
                    }
                    else if (parsed_command[0] == "editAge")
                    {
                        string login = parsed_command[1];
                        int age = Int32.Parse(parsed_command[2]);
                        editAge(login, age);
                    }
                    else if (parsed_command[0] == "display")
                    {
                        DisplayUsers();
                    }
                    else {
                        Console.WriteLine("\nYour command was incorrent, please try again\n");
                    }
                }
                
            }
        }

        public static void addUser(string login, string password, string name, int age)
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = new User { Login = login, Password = GetMD5(password), Name = name, Age = age };
                if (getUser(login) is null)
                {
                    db.Users.Add(user);
                    db.SaveChanges();
                    Console.WriteLine("Insertion was successful");
                }
                else {
                    Console.WriteLine("A user with this login has already been added.");
                }
            }
        }

        public static User getUser(string login)
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

        public static void DisplayUsers()
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                var user_query = db.Users;
                foreach (User user in user_query)
                    Console.WriteLine($"{user.Login}, {user.Password}, {user.Name},  {user.Age}");
            }
        }

        public static void editAge(string login, int age) // изменить возраст
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = getUser(login);
                if (user is null)
                {
                    Console.WriteLine("There is no such login in the database.");
                }
                else {
                    user.Age = age;
                    db.SaveChanges();
                    Console.WriteLine("The edit was successful.");
                }

            }
        }

        public static void removeUser(string login) // удалить элемент
        {
            using (ApplicationContext db = new ApplicationContext())
            {
                User user = getUser(login);
                if (user is not null)
                {
                    db.Users.Remove(user);
                    db.SaveChanges();
                    Console.WriteLine("The removal was successful.");
                }
            }
        }
        static string GetMD5(string password) // шифрование
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