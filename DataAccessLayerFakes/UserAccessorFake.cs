using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessLayerInterfaces;

namespace DataAccessLayerFakes
{
    public class UserAccessorFake : IUserAccessor
    {
        private List<User> fakeUsers = new List<User>();
        private List<string> fakePaswordHashes = new List<string>();
        public UserAccessorFake()
        {
            fakeUsers.Add(new User()
            {
                UserID = 999999,
                UserName = "admin",
                Active = true,
                Roles = new List<string>()
            });
            fakeUsers[0].Roles.Add("Admin");
            fakeUsers[0].Roles.Add("Trusted User");

            fakeUsers.Add(new User()
            {
                UserID = 999998,
                UserName = "fake2",
                Active = true,
                Roles = new List<string>()
            });
            fakeUsers.Add(new User()
            {
                UserID = 999997,
                UserName = "fake3",
                Active = true,
                Roles = new List<string>()
            });

            fakePaswordHashes.Add("8c6976e5b5410415bde908bd4dee15dfb167a9c873fc4bb8a81f6f2ab448a918");
            fakePaswordHashes.Add("bad hash");
            fakePaswordHashes.Add("bad hash");
        }

        public int AddNewUser(string firstName, string lastName, string userName)
        {
            throw new NotImplementedException();
        }

        public int AddUserRole(int userId, string userName, string roleID)
        {
            throw new NotImplementedException();
        }

        public int AuthenticateUserWithUserNameandPasswordHash(string userName, string passwordHash)
        {
            int numAuthenticated = 0;

            // check for user record in fake data
            for (int i = 0; i < fakeUsers.Count; i++)
            {
                if (fakeUsers[i].UserName == userName &&
                    fakePaswordHashes[i] == passwordHash &&
                    fakeUsers[i].Active == true)
                {
                    numAuthenticated++;
                }
            }
            return numAuthenticated;
        }

        public int RemoveUserRole(string userName, string roleID)
        {
            throw new NotImplementedException();
        }

        public List<string> SelectAllUserRoles()
        {
            throw new NotImplementedException();
        }

        public List<string> SelectRolesByUserID(int userID)
        {
            throw new NotImplementedException();
        }

        public User SelectUserByUserName(string userName)
        {
            throw new NotImplementedException();
        }

        public int UpdatePasswordHash(int userID, string passwordHash, string oldPasswordHash)
        {
            throw new NotImplementedException();
        }
    }
}
