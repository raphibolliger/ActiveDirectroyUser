using System;

namespace ActiveDirectoryUser.Model
{
    public class User
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }

        public override int GetHashCode()
        {
            int hash = UserName.GetHashCode();
            return hash;
        }

        public override bool Equals(object obj)
        {
            // If parameter is null return false.
            // ReSharper disable once UseNullPropagation
            if (obj == null)
            {
                return false;
            }

            // If parameter cannot be cast to Point return false.
            User u = obj as User;
            if ((System.Object)u == null)
            {
                return false;
            }

            // Return true if the fields match:
            return UserName.Equals(u.UserName);
        }

        public bool Equals(User u)
        {
            // If parameter is null return false:
            if ((object)u == null)
            {
                return false;
            }

            // Return true if the fields match:
            return UserName.Equals(u.UserName);
        }
    }
}