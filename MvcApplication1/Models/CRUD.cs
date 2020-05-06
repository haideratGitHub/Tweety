using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;

namespace MvcApplication1.Models
{
    public class CRUD
    {
        public static string connectionString = "data source=localhost; Initial Catalog=tweety;Integrated Security=true";

        public static int Login(string username, string password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("user_login", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = password;

                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);
            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = -1; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;
        }

        public static User view_user(String username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("view_user", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();

                //List<User> list = new List<User>();
                //while (rdr.Read())
                if (rdr.Read())
                {
                    User user = new User();

                    user.username = rdr["name"].ToString();
                    user.password = rdr["password"].ToString();
                    user.display_pic = rdr["displayPic"].ToString();
                    user.first_name = rdr["fname"].ToString();
                    user.last_name = rdr["lname"].ToString();
                    user.gender = rdr["gender"].ToString();
                    user.DOB = rdr["DOB"].ToString();
                    user.email = rdr["email"].ToString();
                    user.country = rdr["country"].ToString();
                    user.status = rdr["status"].ToString();

                    //list.Add(user);
                    return user;
                }
                rdr.Close();
                con.Close();
                return null;
                //return list;
            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;
            }
        }

        public static int SignUp(String username, String password, String first_name, String last_name, String email, String country, String gender/*, String BirthDay, String BirthMonth, String BirthYear*/)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("user_signup", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = password;
                cmd.Parameters.Add("@fname", SqlDbType.NVarChar, 30).Value = first_name;
                cmd.Parameters.Add("@lname", SqlDbType.NVarChar, 30).Value = last_name;
                cmd.Parameters.Add("@email", SqlDbType.NVarChar, 30).Value = email;
                cmd.Parameters.Add("@country", SqlDbType.NVarChar, 30).Value = country;
                cmd.Parameters.Add("@gender", SqlDbType.Char, 1).Value = gender;
                //cmd.Parameters.Add("@dateOfBirth", SqlDbType.Date).Value = dateOfBirth;

                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);
            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                result = -1; //-1 will be interpreted as "error while connecting with the database."
            }
            finally
            {
                con.Close();
            }
            return result;
        }
    }
}