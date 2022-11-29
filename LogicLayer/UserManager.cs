using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using DataAccessLayerInterfaces;
using DataObjects;
using LogicLayerInterfaces;

namespace LogicLayer
{
    public class UserManager : IUserManager
    {
        private IUserAccessor userAccessor = null;
        public UserManager()
        {
            userAccessor = new UserAccessor();
        }

        public UserManager(IUserAccessor ua)
        {
            userAccessor = ua;
        }

        public string HashSha256(string source)
        {
            string result = "";

            // check for missing input
            if (source == "" || source == null)
            {
                throw new ArgumentNullException("Missing Input");
            }

            // create a byte array
            byte[] data;

            // create a .NET hash provider object
            using (SHA256 sha256hasher = SHA256.Create())
            {
                // hash the input
                data = sha256hasher.ComputeHash(
                    Encoding.UTF8.GetBytes(source));
            }

            // create output with a stringbuilder object
            var s = new StringBuilder();

            // loop through the hashed output making characters
            // from the values in the byte array
            for (int i = 0; i < data.Length; i++)
            {
                s.Append(data[i].ToString("x2"));
            }

            // convert the stringbuilder into a string
            result = s.ToString();
            result = result.ToLower();

            return result;
        }

        public User LoginUser(string userName, string password)
        {
            User user = null;

            try
            {
                password = HashSha256(password);
                if (1 == userAccessor.AuthenticateUserWithUserNameandPasswordHash(userName, password))
                {
                    user = userAccessor.SelectUserByUserName(userName);
                    user.Roles = userAccessor.SelectRolesByUserID(user.UserID);
                }
                else
                {
                    throw new ApplicationException("User not found.");
                }
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Bad Username or Password", ex);
            }

            return user;
        }

        public bool ResetPassword(User user, string email, string password, string oldPassword)
        {
            throw new NotImplementedException();
        }
    }
}
