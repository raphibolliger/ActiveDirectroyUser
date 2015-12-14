using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ActiveDirectoryUser.Core;
using ActiveDirectoryUser.Model;

namespace ActiveDirectoryUser.Cmd
{
    class Program
    {
        static void Main(string[] args)
        {
            var userService = new UserService();
            var userList = new List<User>();

            Console.WriteLine("Bitte geben Sie die anzahl Benutzer ein: ");
            var userCountString = Console.ReadLine();
            var userCount = int.Parse(userCountString);
            for (int i = 0; i < userCount; i++)
            {
                var user = userService.LoadUser().Result;
                userList.Add(user);
                Console.WriteLine(user.FirstName+ " " + user.LastName + " ("+user.UserName+")");
            }

            Console.WriteLine();
            Console.WriteLine("Saved all Users to \"C:\\Daten\\jsonUsers.txt\"");
            userService.SaveUserList(userList);

            // don't hide the window
            Console.ReadLine();
        }
    }
}
