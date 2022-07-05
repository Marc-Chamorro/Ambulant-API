using API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API.Data
{
    public class UsersData
    {
        public static bool SignUp(Users uData)
        {
            using (SqlConnection uConnection = new SqlConnection(Connect.pathConnection))
            {
                SqlCommand cmd = new SqlCommand("Users_SignUp", uConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                //cmd.Parameters.AddWithValue("@id", uData.ID);
                cmd.Parameters.AddWithValue("@lastName", uData.LastName);
                cmd.Parameters.AddWithValue("@firstName", uData.FirstName);
                cmd.Parameters.AddWithValue("@email", uData.Email);
                cmd.Parameters.AddWithValue("@userPassword", uData.UserPassword);

                try
                {
                    uConnection.Open();
                    cmd.ExecuteNonQuery();
                    return true;
                }
                catch (Exception ex)
                {
                    return false;
                }
            }
        }

        public static List<Users> List()
        {
            List<Users> uListUsers = new List<Users>();
            using (SqlConnection uConnection = new SqlConnection(Connect.pathConnection))
            {
                SqlCommand cmd = new SqlCommand("Users_List", uConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    uConnection.Open();
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            uListUsers.Add(new Users()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                LastName = dr["LastName"].ToString(),
                                FirstName = dr["FirstName"].ToString(),
                                Email = dr["Email"].ToString(),
                                UserPassword = dr["UserPassword"].ToString()
                            });
                        }
                    }
                    return uListUsers;
                }
                catch (Exception ex)
                {
                    return uListUsers;
                }
            }
        }
    }
}