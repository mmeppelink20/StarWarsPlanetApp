using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;


namespace LogicLayerInterfaces
{
    public interface IUserManager
    {
        User LoginUser(string userName, string password);

        string HashSha256(string source);

        bool ResetPassword(User user, string email, string password, string oldPassword);

        List<string> RetrieveAllUserRoles();

        bool FindUser(string userName);

        int RetrieveUserIDFromUserName(string username);

        bool AddUserRole(int userID, string userName, string roleID);
        bool DeleteUserRole(string userName, string roleID);
    }
}
