using DataObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayerInterfaces
{
    public interface IUserAccessor
    {
        int AuthenticateUserWithUserNameandPasswordHash(string userName, string passwordHash);

        User SelectUserByUserName(string userName);

        List<string> SelectRolesByUserID(int userID);

        int UpdatePasswordHash(int userID, string passwordHash, string oldPasswordHash);
        List<string> SelectAllUserRoles();

        int AddNewUser(string firstName, string lastName, string userName);

        int AddUserRole(int userId, string userName, string roleID);

        int RemoveUserRole(string userName, string roleID);
    }
}
