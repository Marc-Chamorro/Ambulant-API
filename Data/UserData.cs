using API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API.Data
{
    public class UserData
    {
        public static List<PersonReduced> List()
        {
            List<PersonReduced> uListUser = new List<PersonReduced>();
            using (SqlConnection uConnection = new SqlConnection(Connect.pathConnection))
            {
                SqlCommand cmd = new SqlCommand("User_List", uConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    uConnection.Open();
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            uListUser.Add(new PersonReduced()
                            {
                                ID = dr["id_persons"].ToString(),
                                Name = dr["name"].ToString(),
                                Email = dr["email"].ToString()
                            });
                        }
                    }
                    return uListUser;
                }
                catch (Exception ex)
                {
                    return uListUser;
                }
            }
        }

        public static bool SignUp(PersonSignUp person)
        {
            using (SqlConnection uConnection = new SqlConnection(Connect.pathConnection))
            {
                SqlCommand cmd = new SqlCommand("User_Creation", uConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", person.Name);
                cmd.Parameters.AddWithValue("@email", person.Email);
                cmd.Parameters.AddWithValue("@password", person.Password);
                cmd.Parameters.AddWithValue("@hash", person.Hash);

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
    }
}
