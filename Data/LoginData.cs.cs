using API.Models;
using API.Security;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace API.Data
{
    public class LoginData
    {
        public static bool SignIn(LoginRequest loginRequest)
        {
            //List<Models.Person> uListUsers = new List<Models.Person>();
            Models.Person mPerson = new Models.Person();
            using (SqlConnection uConnection = new SqlConnection(Connect.pathConnection))
            {
                SqlCommand cmd = new SqlCommand("User_Check_Login", uConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                cmd.Parameters.AddWithValue("@name", loginRequest.Username);

                try
                {
                    uConnection.Open();
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            mPerson.Name = loginRequest.Username;
                            mPerson.Password = dr["password"].ToString();
                            mPerson.Hash = dr["hash"].ToString();
                        }
                    }
                }
                catch (Exception ex)
                {
                    return false;
                }
            }

            string encrypterPsswd = Encryption.ComputeHash(loginRequest.Password, mPerson.Hash);
            if (encrypterPsswd.Equals(mPerson.Password))
            {
                return true;
            }
            return false;
        }
    }
}