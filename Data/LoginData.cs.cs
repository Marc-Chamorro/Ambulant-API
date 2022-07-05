using API.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography;
using System.Web;

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

            string encrypterPsswd = ComputeHash(loginRequest.Password, mPerson.Hash);
            if (encrypterPsswd.Equals(mPerson.Password))
            {
                return true;
            }
            return false;
        }

        private static string ComputeHash(string password, string hash)
        {
            int hashByteSize = Int32.Parse(ConfigurationManager.AppSettings["CRPT_HASH_SIZE"]);
            int iterations = Int32.Parse(ConfigurationManager.AppSettings["CRPT_HASH_ITERATIONS"]);
            byte[] salt = Convert.FromBase64String(hash);

            Rfc2898DeriveBytes hashGenerator = new Rfc2898DeriveBytes(password, salt);

            hashGenerator.IterationCount = iterations;

            return Convert.ToBase64String(hashGenerator.GetBytes(hashByteSize));
        }

    }
}