using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Threading.Tasks;
using System.Windows;
using ActiveDirectoryUser.Core.Helper;
using ActiveDirectoryUser.Model;

namespace ActiveDirectoryUser.Core
{
    public class AdUserService
    {
        private static string ADPath = "LDAP://prog.foresta.net/OU=Testuser,DC=prog,DC=foresta,DC=net";

        private static DirectoryEntry GetDirectoryEntry()
        {
            return new DirectoryEntry(ADPath);
        }

        public static bool UserIsExist(string userName)
        {
            var deUser = GetUserByName(userName);
            return deUser != null ? true : false;
        }

        public static DirectoryEntry GetUserByName(string userName)
        {
            var de = GetDirectoryEntry();
            try
            {
                if (string.IsNullOrEmpty(userName)) throw new ArgumentNullException("Username is null or empty!");
                DirectorySearcher deSearch = new DirectorySearcher();
                deSearch.SearchRoot = de;
                deSearch.Filter = string.Format("(&(objectClass=user)(sAMAccountName={0}))", userName);
                var result = deSearch.FindOne();
                if (result != null)
                {
                    return result.GetDirectoryEntry();
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                de.Dispose();
            }
        }

        public static bool AddUser(User adUser)
        {
            var de = GetDirectoryEntry();
            var deUser = de.Children.Add(string.Format("CN={0}", adUser.FirstName + " " + adUser.LastName), "User");

            deUser.Properties["sAMAccountName"].Value = adUser.UserName;
            deUser.Properties["userPrincipalName"].Value = string.Format("{0}@{1}", adUser.UserName, "prog.foresta.net");
            deUser.Properties["displayName"].Value = adUser.FirstName + " " + adUser.LastName;
            deUser.Properties["name"].Value = adUser.FirstName + " " + adUser.LastName;
            deUser.Properties["givenName"].Value = adUser.FirstName;
            deUser.Properties["sn"].Value = adUser.LastName;

            deUser.CommitChanges();

            ChangePassword(adUser.UserName, "", adUser.Password);
            EnableUser(adUser.UserName);
            deUser.Dispose();
            de.Dispose();
            return true;
        }

        public static bool DeleteUser(string userName)
        {
            var deUser = GetUserByName(userName);
            if (deUser == null) throw new Exception("User dese not exist!");
            try
            {
                deUser.DeleteTree();
                deUser.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                deUser.Dispose();
            }
        }
        public static bool EnableUser(string userName)
        {
            var deUser = GetUserByName(userName);
            if (deUser == null) throw new Exception("User dese not exist!");
            try
            {
                deUser.Properties["userAccountControl"][0] = ADS_USER_FLAG_ENUM.ADS_UF_NORMAL_ACCOUNT | ADS_USER_FLAG_ENUM.ADS_UF_DONT_EXPIRE_PASSWD;
                deUser.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                deUser.Dispose();
            }
        }
        public static bool ChangePassword(string userName, string oldPassword, string password)
        {
            var deUser = GetUserByName(userName);
            if (deUser == null) throw new Exception("User dese not exist!");
            try
            {
                deUser.Invoke("ChangePassword", new object[] { oldPassword, password });
                deUser.CommitChanges();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                deUser.Dispose();
            }
        }

        public static IEnumerable<string> LoadAllUsersFromAd()
        {
            var de = GetDirectoryEntry();
            var deChlidren = de.Children.GetEnumerator();

            while (deChlidren.MoveNext())
            {
                var user = deChlidren.Current as DirectoryEntry;
                yield return (string)user.Properties["sAMAccountName"].Value;
            }
        }

    }
}