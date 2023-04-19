using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataObjects;
using DataAccessLayerInterfaces;
using System.Data.SqlClient;
using System.Data;

namespace DataAccessLayer
{
    public class UserAccessor : IUserAccessor
    {
        public int AuthenticateUserWithUserNameandPasswordHash(string userName, string passwordHash)
        {
            int result = 0;

            // ADO.NET needs a connection
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            // next we need command text
            var cmdText = "sp_authenticate_user";

            // use the command text and connection to create a command object
            var cmd = new SqlCommand(cmdText, conn);

            // set the command type
            cmd.CommandType = CommandType.StoredProcedure;

            // add parameter objects to the command
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@PasswordHash", SqlDbType.NVarChar, 100);

            // set the values for the parameter objects
            cmd.Parameters["@UserName"].Value = userName;
            cmd.Parameters["@PasswordHash"].Value = passwordHash;

            // now that the command is set up, we can invoke it 
            // in a try-catch block
            try
            {
                // open the connection
                conn.Open();

                // execute the command appropriately, and capture the results
                // you can ExecuteScalar, ExecuteNonQuery, or ExecuteReader
                // depending on whether you expect a single value, an int
                // for rows affected, or rows and columns

                result = Convert.ToInt32(cmd.ExecuteScalar());
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close the connection
                conn.Close();
            }

            return result;
        }

        public List<string> SelectAllUserRoles()
        {
            List<string> roles = new List<string>();

            // connection
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            // command text
            var cmdText = "sp_select_all_user_roles";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;


            // try-catch-finally
            try
            {
                conn.Open();

                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close the connection
                conn.Close();
            }
            return roles;
        }

        public List<string> SelectRolesByUserID(int userID)
        {
            List<string> roles = new List<string>();

            // connection
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            // command text
            var cmdText = "sp_select_roles_by_userID";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@UserID", SqlDbType.Int);

            // values
            cmd.Parameters["@UserID"].Value = userID;

            // try-catch-finally
            try
            {
                // open the connection
                conn.Open();

                // execute the command
                var reader = cmd.ExecuteReader();

                // process the results
                if (reader.HasRows)
                {
                    while (reader.Read())
                    {
                        roles.Add(reader.GetString(0));
                    }
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close the connection
                conn.Close();
            }
            return roles;
        }

        public User SelectUserByUserName(string userName)
        {
            User user = null;

            // connection
            DBConnection connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            // command text
            var cmdText = "sp_select_user_by_username";

            // command
            var cmd = new SqlCommand(cmdText, conn);

            // command type
            cmd.CommandType = CommandType.StoredProcedure;

            // parameters
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 50);

            // values
            cmd.Parameters["@UserName"].Value = userName;

            // try-catch-finally
            try
            {
                // open
                conn.Open();

                // execute and get a SqlDataReader
                var reader = cmd.ExecuteReader();

                if (reader.HasRows)
                {
                    // most of the time there will be a while loop
                    // here, we don't need it

                    reader.Read();
                    //[UserID], [UserName], [Active]

                    user = new User();

                    user.UserID = reader.GetInt32(0);
                    user.UserName = reader.GetString(1);
                    user.FirstName = reader.GetString(2);
                    user.FamilyName = reader.GetString(3);
                    user.Active = reader.GetBoolean(4);
                }
                // close the reader
                reader.Close();
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                // close
                conn.Close();
            }

            return user;
        }

        public int AddNewUser(string firstName, string lastName, string userName)
        {

            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_insert_user";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@FirstName", SqlDbType.NVarChar, 50);
            cmd.Parameters.Add("@FamilyName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100);


            cmd.Parameters["@FirstName"].Value = firstName;
            cmd.Parameters["@FamilyName"].Value = lastName;
            cmd.Parameters["@UserName"].Value = userName;

            try
            {
                conn.Open();


                result = Convert.ToInt32(cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();
                
            }
            return result;
        }

        public int UpdatePasswordHash(int userID, string passwordHash, string oldPasswordHash)
        {
            throw new NotImplementedException();
        }

        public int AddUserRole(int userId, string userName, string roleID)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_add_user_roles";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserID", SqlDbType.Int);
            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 50);


            cmd.Parameters["@UserID"].Value = userId;
            cmd.Parameters["@UserName"].Value = userName;
            cmd.Parameters["@RoleID"].Value = roleID;

            try
            {
                conn.Open();


                result = Convert.ToInt32(cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();

            }
            return result;
        }

        public int RemoveUserRole(string userName, string roleID)
        {
            int result = 0;

            var connectionFactory = new DBConnection();
            var conn = connectionFactory.GetConnection();

            var cmdText = "sp_remove_user_roles";

            var cmd = new SqlCommand(cmdText, conn);

            cmd.CommandType = CommandType.StoredProcedure;

            cmd.Parameters.Add("@UserName", SqlDbType.NVarChar, 100);
            cmd.Parameters.Add("@RoleID", SqlDbType.NVarChar, 50);


            cmd.Parameters["@UserName"].Value = userName;
            cmd.Parameters["@RoleID"].Value = roleID;

            try
            {
                conn.Open();


                result = Convert.ToInt32(cmd.ExecuteNonQuery());

            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                conn.Close();

            }
            return result;
        }
    }
}
