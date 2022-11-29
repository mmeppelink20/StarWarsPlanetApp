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
                    user.Active = reader.GetBoolean(2);
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

        public int UpdatePasswordHash(int userID, string passwordHash, string oldPasswordHash)
        {
            throw new NotImplementedException();
        }
    }
}
