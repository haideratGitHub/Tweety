using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Data.SqlClient;
using System.Data.Sql;
using System.Data;
using System.Web.Services.Description;

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

                    if (user.display_pic == "")
                        user.display_pic = "https://herbalforlife.co.uk/wp-content/uploads/2019/08/user-placeholder.png";
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
        public static int no_of_followers(string username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("no_of_followers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    result = Convert.ToInt32(rdr["followers"].ToString());
                    return result;
                }
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

        public static int no_of_followings(string username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("no_of_followings", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    result = Convert.ToInt32(rdr["following"].ToString());
                    return result;
                }
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

        public static List<User> get_followers(String username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("followers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();

                List<User> list = new List<User>();
                while (rdr.Read())
                {
                    User user = new User();

                    user.username = rdr["follower"].ToString();
                    user.display_pic = rdr["displayPic"].ToString();
                    user.first_name = rdr["fname"].ToString();
                    user.last_name = rdr["lname"].ToString();
                    user.gender = rdr["gender"].ToString();
                    user.country = rdr["country"].ToString();
                    user.status = rdr["status"].ToString();

                    if (user.display_pic == "")
                        user.display_pic = "https://herbalforlife.co.uk/wp-content/uploads/2019/08/user-placeholder.png";
                    list.Add(user);
                }
                rdr.Close();
                con.Close();
                return list;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;
            }
        }

        public static List<User> get_following(String username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("followings", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();

                List<User> list = new List<User>();
                while (rdr.Read())
                {
                    User user = new User();

                    user.username = rdr["following"].ToString();
                    user.display_pic = rdr["displayPic"].ToString();
                    user.first_name = rdr["fname"].ToString();
                    user.last_name = rdr["lname"].ToString();
                    user.gender = rdr["gender"].ToString();
                    user.country = rdr["country"].ToString();
                    user.status = rdr["status"].ToString();

                    if (user.display_pic == "")
                        user.display_pic = "https://herbalforlife.co.uk/wp-content/uploads/2019/08/user-placeholder.png";
                    list.Add(user);
                }
                rdr.Close();
                con.Close();
                return list;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;
            }
        }

        public static int ToFollow(string username, string toFollow)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("follow", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@other_username", SqlDbType.NVarChar, 30).Value = toFollow;

                //cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                //result = Convert.ToInt32(cmd.Parameters["@output"].Value);
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

        public static int ToUnFollow(string username, string toUnFollow)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("unfollow", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@other_username", SqlDbType.NVarChar, 30).Value = toUnFollow;

                //cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                //result = Convert.ToInt32(cmd.Parameters["@output"].Value);
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

        public static int RemoveFollower(string username, string toRemove)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("remove_follower", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@follower", SqlDbType.NVarChar, 30).Value = toRemove;
                //cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                //result = Convert.ToInt32(cmd.Parameters["@output"].Value);
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


        public static List<User> People_U_Should_Follow(string username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("People_U_Should_Follow", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();

                List<User> list = new List<User>();
                while (rdr.Read())
                {
                    User user = new User();

                    user.username = rdr["name"].ToString();
                    user.display_pic = rdr["displayPic"].ToString();
                    user.first_name = rdr["fname"].ToString();
                    user.last_name = rdr["lname"].ToString();
                    if (user.display_pic == "")
                        user.display_pic = "https://herbalforlife.co.uk/wp-content/uploads/2019/08/user-placeholder.png";
                    list.Add(user);
                }
                rdr.Close();
                con.Close();

                return list;


            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }

        public static List<hashtag_trending> trending_hashtag()
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("trending_hashtag", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                SqlDataReader rdr = cmd.ExecuteReader();

                List<hashtag_trending> list = new List<hashtag_trending>();
                while (rdr.Read())
                {
                    hashtag_trending hashTag = new hashtag_trending();

                    hashTag.hashtag = rdr["hashtag"].ToString();
                    hashTag.count = rdr["count_#"].ToString();
                    list.Add(hashTag);
                }
                rdr.Close();
                con.Close();

                return list;


            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }

        public static int no_of_tweets(string username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("no_of_tweets", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    result = Convert.ToInt32(rdr["tweets"].ToString());
                    return result;
                }
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

        public static int no_of_likes(string username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("no_of_likes", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    result = Convert.ToInt32(rdr["likes"].ToString());
                    return result;
                }
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

        public static int no_of_dislikes(string username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("no_of_dislikes", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    result = Convert.ToInt32(rdr["dislikes"].ToString());
                    return result;
                }
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

        public static List<Tweet> tweets_of_a_user(String username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("tweets_of_user", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();

                List<Tweet> list = new List<Tweet>();
                int i = 0, j = -1;
                while (rdr.Read())
                {
                    Tweet tweet = new Tweet();

                    tweet.tweetID = rdr["tweetID"].ToString();
                    tweet.userID = rdr["userID"].ToString();
                    tweet.tweet = rdr["tweet"].ToString();
                    tweet.date = rdr["date"].ToString();
                    tweet.time = rdr["time"].ToString();

                    tweet.no_of_likes = no_of_likes_on_a_tweet(Convert.ToInt32(tweet.tweetID));
                    tweet.no_of_dislikes = no_of_dislikes_on_a_tweet(Convert.ToInt32(tweet.tweetID));
                    tweet.no_of_comments = no_of_comments_on_a_tweet(Convert.ToInt32(tweet.tweetID));
                    tweet.likers = likers_of_a_tweet(Convert.ToInt32(tweet.tweetID));
                    tweet.dislikers = dislikers_of_a_tweet(Convert.ToInt32(tweet.tweetID));
                    tweet.comments = comments_on_a_tweet(Convert.ToInt32(tweet.tweetID));

                    //FOR MODALS USE
                    tweet.modelID1 = i;
                    tweet.modelID2 = j;
                    i = i - 2;
                    j = j - 2;
                    list.Add(tweet);
                }
                rdr.Close();
                con.Close();
                return list;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;
            }
        }

        public static int no_of_likes_on_a_tweet(int tweetID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("no_of_likes_on_a_tweet", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@tweet_id", SqlDbType.Int).Value = tweetID;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    result = Convert.ToInt32(rdr["likes"].ToString());
                    return result;
                }
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

        public static int no_of_dislikes_on_a_tweet(int tweetID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("no_of_dislikes_on_a_tweet", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@tweet_id", SqlDbType.Int).Value = tweetID;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    result = Convert.ToInt32(rdr["dislikes"].ToString());
                    return result;
                }
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

        public static int no_of_comments_on_a_tweet(int tweetID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("no_of_comments_on_a_tweet", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@tweet_id", SqlDbType.Int).Value = tweetID;

                SqlDataReader rdr = cmd.ExecuteReader();
                if (rdr.Read())
                {
                    result = Convert.ToInt32(rdr["comments"].ToString());
                    return result;
                }
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

        public static List<User> likers_of_a_tweet(int tweetID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("likers_of_a_tweet", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@tweet_id", SqlDbType.Int).Value = tweetID;

                SqlDataReader rdr = cmd.ExecuteReader();
                string liker_name;
                List<User> list = new List<User>();
                while (rdr.Read())
                {
                    User user = new User();

                    liker_name = rdr["liked_by_users"].ToString();
                    user = view_user(liker_name);

                    list.Add(user);
                }
                rdr.Close();
                con.Close();
                return list;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;
            }
        }

        public static List<User> dislikers_of_a_tweet(int tweetID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("dislikers_of_a_tweet", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@tweet_id", SqlDbType.Int).Value = tweetID;

                SqlDataReader rdr = cmd.ExecuteReader();
                string disliker_name;
                List<User> list = new List<User>();
                while (rdr.Read())
                {
                    User user = new User();

                    disliker_name = rdr["disliked_by_users"].ToString();
                    user = view_user(disliker_name);

                    list.Add(user);
                }
                rdr.Close();
                con.Close();
                return list;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;
            }
        }

        public static List<Comment> comments_on_a_tweet(int tweetID)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("comments_on_a_tweet", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@tweet_id", SqlDbType.Int).Value = tweetID;

                SqlDataReader rdr = cmd.ExecuteReader();
                string commenterName;
                List<Comment> list = new List<Comment>();
                while (rdr.Read())
                {
                    Comment c = new Comment();

                    c.tweetID = rdr["tweetID"].ToString();
                    c.commentID = rdr["commentID"].ToString();
                    c.commenterID = rdr["commenterID"].ToString();
                    c.date = rdr["date"].ToString();
                    c.time = rdr["time"].ToString();
                    c.comment = rdr["comment"].ToString();
                    commenterName = rdr["name"].ToString();

                    c.commenter = view_user(commenterName);

                    list.Add(c);
                }
                rdr.Close();
                con.Close();
                return list;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;
            }
        }

        public static List<Notification> showNotifications(string username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("showNotifications", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();

                List<Notification> list = new List<Notification>();
                while (rdr.Read())
                {
                    Notification notification = new Notification();

                    notification.notificationID = rdr["notificationID"].ToString();
                    notification.userID = rdr["userID"].ToString();
                    notification.nDate = rdr["nDate"].ToString();
                    notification.nTime = rdr["nTime"].ToString();
                    notification.readFlag = rdr["readFlag"].ToString();
                    notification.n_Text = rdr["n_Text"].ToString();
                    list.Add(notification);
                }
                rdr.Close();
                con.Close();

                return list;


            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }

        }
        public static List<Messages> showMessages(string senderName, string receverName)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("chat_out", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;

                cmd.Parameters.Add("@sender", SqlDbType.NVarChar, 30).Value = senderName;
                cmd.Parameters.Add("@recever", SqlDbType.NVarChar, 30).Value = receverName;


                SqlDataReader rdr = cmd.ExecuteReader();

                List<Messages> list = new List<Messages>();
                while (rdr.Read())
                {
                    Messages message = new Messages();

                    message.senderName = rdr["sender"].ToString();
                    message.receverName = rdr["recever"].ToString();
                    message.message = rdr["message"].ToString();
                    message.time = rdr["date"].ToString();
                    list.Add(message);
                }
                rdr.Close();
                con.Close();

                return list;


            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;

            }





        }

        public static void storeMessage(string senderName, string receverName, string message)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;


            try
            {
                cmd = new SqlCommand("chat_in", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@sender", SqlDbType.NVarChar, 30).Value = senderName;
                cmd.Parameters.Add("@receiver", SqlDbType.NVarChar, 30).Value = receverName;
                cmd.Parameters.Add("@message", SqlDbType.NVarChar, 280).Value = message;





                cmd.ExecuteNonQuery();

            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());

            }
            finally
            {
                con.Close();
            }
        }

        public static int Update_FirstName(String username, String first_name, String password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("change_first_name", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = password;
                cmd.Parameters.Add("@new", SqlDbType.NVarChar, 30).Value = first_name;

                cmd.ExecuteNonQuery();

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
        public static int Update_LastName(String username, String last_name, String password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("change_last_name", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = password;
                cmd.Parameters.Add("@new", SqlDbType.NVarChar, 30).Value = last_name;

                cmd.ExecuteNonQuery();

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
        public static int Update_Gender(String username, String gender, String password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("change_gender", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = password;
                cmd.Parameters.Add("@new", SqlDbType.Char).Value = gender;

                cmd.ExecuteNonQuery();

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
        public static int Update_Email(String username, String email, String password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("change_email", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = password;
                cmd.Parameters.Add("@new", SqlDbType.NVarChar, 30).Value = email;

                cmd.ExecuteNonQuery();

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
        public static int Update_Country(String username, String country, String password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("change_country", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = password;
                cmd.Parameters.Add("@new", SqlDbType.NVarChar, 30).Value = country;

                cmd.ExecuteNonQuery();

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
        public static int Update_Status(String username, String status, String password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("change_status", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = password;
                cmd.Parameters.Add("@new", SqlDbType.NVarChar, 120).Value = status;

                cmd.ExecuteNonQuery();

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
        public static int Update_UserName(String old_username, String new_username, String password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("change_username", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@old", SqlDbType.NVarChar, 30).Value = old_username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = password;
                cmd.Parameters.Add("@new", SqlDbType.NVarChar, 30).Value = new_username;
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
        public static int Update_Password(String username, String new_password, String old_password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("change_password", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@name", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = old_password;
                cmd.Parameters.Add("@new", SqlDbType.NVarChar, 30).Value = new_password;
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
        public static int Update_DisplayPic(String username, String DP, String password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("change_displayPic", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = password;
                cmd.Parameters.Add("@new", SqlDbType.NVarChar, 1000).Value = DP;

                cmd.ExecuteNonQuery();

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
        public static int Update_DOB(String username, String DOB, String password)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("change_DOB", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@password", SqlDbType.NVarChar, 30).Value = password;
                cmd.Parameters.Add("@new", SqlDbType.Date).Value = DOB;

                cmd.ExecuteNonQuery();

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

        public static int like_a_tweet(int tweetID, string username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("like_a_tweet", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@liker", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@tweet_id", SqlDbType.Int).Value = tweetID;
                cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output"].Value);
                //@output = 2 means already liked tweet 
                if (result == 2)
                {
                    cmd = new SqlCommand("unlike_a_tweet", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@liker", SqlDbType.NVarChar, 30).Value = username;
                    cmd.Parameters.Add("@tweet_id", SqlDbType.Int).Value = tweetID;
                    cmd.ExecuteNonQuery();
                }
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

        public static int dislike_a_tweet(int tweetID, string username)
        {

            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("dislike_a_tweet", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@disliker", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@tweet_id", SqlDbType.Int).Value = tweetID;
                cmd.Parameters.Add("@output1", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                result = Convert.ToInt32(cmd.Parameters["@output1"].Value);
                //@output = 2 means already unliked tweet 
                if (result == 2)
                {
                    cmd = new SqlCommand("undislike_a_tweet", con);
                    cmd.CommandType = System.Data.CommandType.StoredProcedure;
                    cmd.Parameters.Add("@disliker", SqlDbType.NVarChar, 30).Value = username;
                    cmd.Parameters.Add("@tweet_id", SqlDbType.Int).Value = tweetID;
                    cmd.ExecuteNonQuery();
                }
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

        public static List<User> show_search_list_of_users(String text)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("search_user", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@text", SqlDbType.NVarChar, 140).Value = text;

                cmd.ExecuteNonQuery();

                SqlDataReader rdr = cmd.ExecuteReader();
                List<User> list = new List<User>();
                while (rdr.Read())
                {
                    User user = new User();

                    user.username = rdr["name"].ToString();
                    user.country = rdr["country"].ToString();
                    user.DOB = rdr["country"].ToString();
                    user.email = rdr["email"].ToString();
                    user.gender = rdr["gender"].ToString();
                    user.status = rdr["status"].ToString();
                    user.display_pic = rdr["displayPic"].ToString();
                    user.first_name = rdr["fname"].ToString();
                    user.last_name = rdr["lname"].ToString();
                    if (user.display_pic == "")
                        user.display_pic = "https://herbalforlife.co.uk/wp-content/uploads/2019/08/user-placeholder.png";
                    list.Add(user);
                }
                rdr.Close();
                con.Close();

                return list;


            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;
            }
        }

        public static int postTweet(string tweet, string username, string[] hashtags)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd1; //to post tweet
            int result = 0;

            try
            {
                cmd1 = new SqlCommand("tweet", con);
                cmd1.CommandType = System.Data.CommandType.StoredProcedure;
                cmd1.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd1.Parameters.Add("@tweet", SqlDbType.NVarChar, 280).Value = tweet;
                cmd1.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd1.ExecuteNonQuery();
                result = Convert.ToInt32(cmd1.Parameters["@output"].Value);
                //result will return tweetID of new tweet which is used to add hashtags
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


        public static int commentOnTweet(int tweetID, string commentText, string username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("comment", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;
                cmd.Parameters.Add("@tweet_id", SqlDbType.Int).Value = tweetID;
                cmd.Parameters.Add("@comment", SqlDbType.NVarChar, 280).Value = commentText;
                //cmd.Parameters.Add("@output", SqlDbType.Int).Direction = ParameterDirection.Output;

                cmd.ExecuteNonQuery();
                //result = Convert.ToInt32(cmd.Parameters["@output"].Value);
                
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

        public static int delete_User(string username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            int result = 0;

            try
            {
                cmd = new SqlCommand("delete_user", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                cmd.ExecuteNonQuery();
                
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


        public static List<User> show_search_list_of_following(String text, String Username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("explore_following", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@text", SqlDbType.NVarChar, 140).Value = text;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = Username;

                cmd.ExecuteNonQuery();

                SqlDataReader rdr = cmd.ExecuteReader();
                List<User> list = new List<User>();
                while (rdr.Read())
                {
                    User user = new User();

                    user.username = rdr["name"].ToString();
                    user.country = rdr["country"].ToString();
                    user.gender = rdr["gender"].ToString();
                    user.status = rdr["status"].ToString();
                    user.display_pic = rdr["displayPic"].ToString();
                    user.first_name = rdr["fname"].ToString();
                    user.last_name = rdr["lname"].ToString();
                    if (user.display_pic == "")
                        user.display_pic = "https://herbalforlife.co.uk/wp-content/uploads/2019/08/user-placeholder.png";
                    list.Add(user);
                }
                rdr.Close();
                con.Close();

                return list;


            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;
            }
        }


        public static List<User> show_search_list_of_strangers(String text, String Username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;
            try
            {
                cmd = new SqlCommand("explore_strangers", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@text", SqlDbType.NVarChar, 140).Value = text;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = Username;

                cmd.ExecuteNonQuery();

                SqlDataReader rdr = cmd.ExecuteReader();
                List<User> list = new List<User>();
                while (rdr.Read())
                {
                    User user = new User();

                    user.username = rdr["name"].ToString();
                    user.country = rdr["country"].ToString();
                    user.gender = rdr["gender"].ToString();
                    user.status = rdr["status"].ToString();
                    user.display_pic = rdr["displayPic"].ToString();
                    user.first_name = rdr["fname"].ToString();
                    user.last_name = rdr["lname"].ToString();
                    if (user.display_pic == "")
                        user.display_pic = "https://herbalforlife.co.uk/wp-content/uploads/2019/08/user-placeholder.png";
                    list.Add(user);
                }
                rdr.Close();
                con.Close();

                return list;


            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;
            }
        }

        public static void addHashtags(int tweetID, string[] hashtags)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd; //to add hashtags of tweet

            try
            {
                cmd = new SqlCommand("addHashtags", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@tweetID", SqlDbType.Int).Value = tweetID;
                for (int i = 0; i < hashtags.Length; i++)
                {
                    if (hashtags[i] != "")
                    {
                        string h = hashtags[i];
                        cmd.Parameters.Add("@hashtag", SqlDbType.NVarChar, 50).Value = h;
                        cmd.ExecuteNonQuery();
                    }

                }

            }

            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
            }
            finally
            {
                con.Close();
            }
        }


        public static List<User> get_followers_and_following(String username)
        {
            SqlConnection con = new SqlConnection(connectionString);
            con.Open();
            SqlCommand cmd;

            try
            {
                cmd = new SqlCommand("get_followers_and_following", con);
                cmd.CommandType = System.Data.CommandType.StoredProcedure;
                cmd.Parameters.Add("@username", SqlDbType.NVarChar, 30).Value = username;

                SqlDataReader rdr = cmd.ExecuteReader();

                List<User> list = new List<User>();
                while (rdr.Read())
                {
                    User user = new User();

                    user.username = rdr["name"].ToString();
                    list.Add(user);
                }
                rdr.Close();
                con.Close();
                return list;
            }
            catch (SqlException ex)
            {
                Console.WriteLine("SQL Error" + ex.Message.ToString());
                return null;
            }
        }



    }
}
