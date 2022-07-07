using API.Models;
using API.Security;
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
            //check that the user does not exist
            using (SqlConnection uConnection = new SqlConnection(Connect.pathConnection))
            {
                SqlCommand cmd = new SqlCommand("User_Check_Exists", uConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@name", person.Name);
                cmd.Parameters.AddWithValue("@email", person.Email);

                try
                {
                    uConnection.Open();
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            return false;
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            //create the user
            using (SqlConnection uConnection = new SqlConnection(Connect.pathConnection))
            {
                SqlCommand cmd = new SqlCommand("User_Creation", uConnection);
                cmd.CommandType = CommandType.StoredProcedure;
                cmd.Parameters.AddWithValue("@name", person.Name);
                cmd.Parameters.AddWithValue("@email", person.Email);
                try
                {
                    string hash = Encryption.GenerateSalt();
                    cmd.Parameters.AddWithValue("@hash", hash);

                    string password = Encryption.ComputeHash(person.Password, hash);
                    cmd.Parameters.AddWithValue("@password", password);
                }
                catch (Exception ex)
                {
                    return false;
                }

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
