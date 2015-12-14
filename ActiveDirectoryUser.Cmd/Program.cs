using System;
using System.Collections.Generic;
using System.Linq;
using ActiveDirectoryUser.Core;
using ActiveDirectoryUser.Model;

namespace ActiveDirectoryUser.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var userService = new UserService();
            var userList = new HashSet<User>();

            Console.WriteLine("Bitte geben Sie die anzahl Benutzer ein: ");
            var userCountString = Console.ReadLine();
            var userCount = int.Parse(userCountString);
            while (userList.Count < userCount)
            {
                var user = userService.LoadUser().Result;
                if (userList.Add(user))
                    Console.WriteLine(user.FirstName + " " + user.LastName + " (" + user.UserName + ")");
                else
                    Console.WriteLine("---!!--- "+user.FirstName + " " + user.LastName + " (" + user.UserName + ")");
            }

            Console.WriteLine();
            Console.WriteLine("Saved all Users to \"C:\\Daten\\jsonUsers.txt\"");
            userService.SaveUserList(userList.ToList());

            // don't hide the window
            Console.ReadLine();
        }
    }
}
