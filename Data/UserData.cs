using API.Common_Functions;
using API.Models;
using API.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace API.Data
{
    public class UserData
    {
        public static HttpResponseMessage List(HttpRequestMessage request)
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
                    return request.CreateResponse(HttpStatusCode.OK, uListUser);
                }
                catch (Exception ex)
                {
                    return request.CreateResponse(HttpStatusCode.InternalServerError, uListUser);
                }
            }
        }

        public static HttpResponseMessage SignUp(HttpRequestMessage request, PersonSignUp person)
        {
            //check for the created user size
            if (person.Name.Length < 3)
            {
                return request.CreateResponse((HttpStatusCode)701, "Short name");
            } else if (person.Email.Length < 2)
            {
                return request.CreateResponse((HttpStatusCode)701, "Short email");
            } else if (person.Password.Length < 5)
            {
                return request.CreateResponse((HttpStatusCode)701, "Short password");
            }

            //check string characters from the asci table 32 to the 126 both included
            TextValidation textValidation = new TextValidation();
            if (!textValidation.CheckAsciiCharacters(person.Name))
            {
                return request.CreateResponse((HttpStatusCode)702, "Invalid name characters");
            } else if (!textValidation.CheckAsciiCharacters(person.Email))
            {
                return request.CreateResponse((HttpStatusCode)702, "Invalid email characters");
            }
            if (!textValidation.CheckAsciiCharacters(person.Password))
            {
                return request.CreateResponse((HttpStatusCode)702, "Invalid password characters");
            }

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
                            //409
                            return request.CreateResponse(HttpStatusCode.Conflict, "User already exists");
                        }
                    }
                }
                catch (Exception ex)
                {
                    //500
                    return request.CreateResponse(HttpStatusCode.InternalServerError, "Error with the database");
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
                    //500
                    return request.CreateResponse(HttpStatusCode.InternalServerError, "Error while encrypting");
                }

                try
                {
                    uConnection.Open();
                    cmd.ExecuteNonQuery();
                    return request.CreateResponse(HttpStatusCode.OK, "User created");
                }
                catch (Exception ex)
                {
                    //500
                    return request.CreateResponse(HttpStatusCode.InternalServerError, "Error with the database");
                }
            }
        }
    }
}
