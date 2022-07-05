using API.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace API.Data
{
    public class TypeData
    {
        public static List<Models.Type> List()
        {
            List<Models.Type> uListUsers = new List<Models.Type>();
            using (SqlConnection uConnection = new SqlConnection(Connect.pathConnection))
            {
                SqlCommand cmd = new SqlCommand("Type_List", uConnection);
                cmd.CommandType = CommandType.StoredProcedure;

                try
                {
                    uConnection.Open();
                    cmd.ExecuteNonQuery();

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            uListUsers.Add(new Models.Type()
                            {
                                ID = Convert.ToInt32(dr["ID"]),
                                Name = dr["Name"].ToString(),
                                Tag = dr["Tag"].ToString(),
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