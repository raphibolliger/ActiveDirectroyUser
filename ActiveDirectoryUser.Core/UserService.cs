using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ActiveDirectoryUser.Model;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ActiveDirectoryUser.Core
{
    public class UserService
    {

        private async Task<string> LoadRandomUser()
        {
            var client = new HttpClient();
            var uri = new Uri("https://randomuser.me/api/");
            return await client.GetStringAsync(uri);
        }

        public async Task<User> LoadUser()
        {
            var jsonUser = LoadRandomUser().Result;
            JObject jUser = JObject.Parse(jsonUser);

            string firstName = (string)jUser["results"][0]["user"]["name"]["first"];
            string lastName = (string)jUser["results"][0]["user"]["name"]["last"];

            var user = new User
            {
                FirstName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(firstName, @"\s+", "")),
                LastName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(Regex.Replace(lastName, @"\s+", "")),
                Password = "itsar2015",
                UserName = firstName+"."+lastName
            };

            return user;
        }

        public void SaveUserList(List<User> userList)
        {
            JsonSerializer serializer = new JsonSerializer();

            using (StreamWriter sw = new StreamWriter(@"C:\Daten\jsonUsers.txt"))
            using (JsonWriter writer = new JsonTextWriter(sw))
            {
                serializer.Serialize(writer, userList);
            }
        }

        public List<User> ReadUserListFromFile(string jsonFilePath)
        {
            List<User> jUserList = new List<User>();
            using (StreamReader r = new StreamReader(jsonFilePath))
            {
                string json = r.ReadToEnd();
                jUserList = JsonConvert.DeserializeObject<List<User>>(json);
            }
            return jUserList;
        }


    }
}