using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Dynamic;
using MvcApplication1.Models;
namespace MvcApplication1.Controllers
{
    public class HomeController : Controller
    {
        static class Globals
        {
           
            public static string recever=null;

        }
        static string Search = null;

        public ActionResult Login(){
            Search = null;
            if (Session["username"] == null)
                return View("Login");
            else
                return RedirectToAction("HomePage");
        }
        public ActionResult SignUp()
        {
            return View();
        }
        public ActionResult StartUp()
        {
            return View();
        }
        public ActionResult Loading()
        {
            return View();
        }
        public ActionResult LogOut(){
            Search = null;
            Session["username"] = null;
            return View("Login");
        } 
       public ActionResult Settings()
        {
            Search = null;
            if (Session["username"] == null)
                return View("login");
            else{
                User user = CRUD.view_user(Session["username"].ToString());
                return View(user);
            }
        }
        public ActionResult Notifications(String username)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                User users = CRUD.view_user(Session["username"].ToString());
                List<User> People_U_Should_Follow_list = CRUD.People_U_Should_Follow(Session["username"].ToString());
                List<hashtag_trending> trendingHashtags = CRUD.trending_hashtag();
                List<Notification> notifications = CRUD.showNotifications(Session["username"].ToString());

                dynamic model = new ExpandoObject();
                model.User = users;
                model.Suggested_people = People_U_Should_Follow_list;
                model.trending_hashtags = trendingHashtags;
                model.Notifications = notifications;

                return View(model);
            }
        }

        public ActionResult Messages(string rec=null)
        {
            Search = null;
            if (Session["username"] == null)
                return View("login");
            else
            {
                User users = CRUD.view_user(Session["username"].ToString());
                List<User> followers = CRUD.get_followers_and_following(Session["username"].ToString());
                List<Messages> Messages;
                if (rec == null && Globals.recever==null)
                {
                    Messages = CRUD.showMessages(Session["username"].ToString(), " ");
                }
                else if(rec == null && Globals.recever != null)
                {
                    
                    Messages = CRUD.showMessages(Session["username"].ToString(), Globals.recever);
                    
                }
                else
                {
                    Globals.recever = rec;
                    Messages = CRUD.showMessages(Session["username"].ToString(), Globals.recever);
                }


                dynamic model = new ExpandoObject();
                model.User = users;
                model.followers = followers;
                model.Messages = Messages;
                return View(model);
            }
        }


        public ActionResult Explore(string search)
        {
            if (Session["username"] == null)
                return View("Login");
            else
            {
                if(search == null)
                {
                    if (Search != null)
                        search = Search;
                    else
                        search = "";
                }
                Search = search;
                User users = CRUD.view_user(Session["username"].ToString());
                List<hashtag_trending> trendingHashtags = CRUD.trending_hashtag();
                List<User> following_of_search = CRUD.show_search_list_of_following(search, Session["username"].ToString());
                List<User> strangers_of_search = CRUD.show_search_list_of_strangers(search, Session["username"].ToString());
                List<User> People_U_Should_Follow_list = CRUD.People_U_Should_Follow(Session["username"].ToString());
                dynamic model = new ExpandoObject();
                model.User = users;
                model.trending_hashtags = trendingHashtags;
                model.Searched_following = following_of_search;
                model.Searched_strangers = strangers_of_search;
                model.Suggested_people = People_U_Should_Follow_list;
                return View(model);
            }
            
        }

        public ActionResult authenticate(String username, String password)
        {
            int result = CRUD.Login(username, password);

            if (result == -1)
            {
                String data = "Something went wrong while connecting with the database.";
                return View("Login", (object)data);
            }
            else if (result == 0)
            {

                String data = "Incorrect Credentials";
                return View("Login", (object)data);
            }
            Session["username"] = username;

            return RedirectToAction("HomePage");
            //return RedirectToAction("HomePage", new { username = username });
        }
        public ActionResult HomePage(String username)
        {
            Search = null;
            if (Session["username"] == null)
                return View("login");
            else
            {
                User users = CRUD.view_user(Session["username"].ToString());
                List<User> People_U_Should_Follow_list = CRUD.People_U_Should_Follow(Session["username"].ToString());
                List<hashtag_trending> trendingHashtags = CRUD.trending_hashtag();

                List<feedObject> feedObjects = new List<feedObject>();
                

                List<User> following = CRUD.get_following(Session["username"].ToString());   
                dynamic model = new ExpandoObject();
                for (int i = 0; i < following.Count; i++)
                {
                    List<Tweet> tweets = CRUD.tweets_of_a_user(following[i].username);
                    for(int j=0; j<tweets.Count; j++)
                    {
                        feedObject fo = new feedObject();
                        fo.display_pic = following[i].display_pic;
                        fo.first_name = following[i].first_name;
                        fo.last_name = following[i].last_name;
                        fo.username = following[i].username;
                        fo.userTweet = tweets[j];
                        feedObjects.Add(fo);
                    }
                    
                }
                feedObjects.OrderByDescending(o => o.userTweet.date).ToList();
                model.User = users;
                model.Suggested_people = People_U_Should_Follow_list;
                model.trending_hashtags = trendingHashtags;
                model.feed = feedObjects;
                   
                return View(model);
            }
        }

        public ActionResult ProfilePage(String username)
        {
            Search = null;
            if (Session["username"] == null)
                return View("login");
            else
            {//User users = CRUD.view_user(Session["username"].ToString());
                dynamic mymodel = new ExpandoObject();
                mymodel.user = CRUD.view_user(Session["username"].ToString());
                mymodel.no_of_followers = CRUD.no_of_followers(Session["username"].ToString());
                mymodel.no_of_followings = CRUD.no_of_followings(Session["username"].ToString());
                mymodel.no_of_tweets = CRUD.no_of_tweets(Session["username"].ToString());
                mymodel.no_of_likes = CRUD.no_of_likes(Session["username"].ToString());
                mymodel.no_of_dislikes = CRUD.no_of_dislikes(Session["username"].ToString());
                mymodel.tweets = CRUD.tweets_of_a_user(Session["username"].ToString());
                Console.Write(mymodel);
                return View(mymodel);
            }
        }

        public ActionResult Add_User(String username, String password, String first_name, String last_name, String email, String country, String gender/*,String BirthDay, String BirthMonth, String BirthYear*/ )
        {
            int result = CRUD.SignUp(username, password, first_name, last_name, email, country, gender);
            if (result == -1)
            {
                String data = "Something went wrong while connecting with the database.";
                return View("Login", (object)data);
            }
            else if (result == 0)
            {
                String data = "This Username is already in use";
                return View("SignUp", (object)data);
            }
            Session["username"] = username;
            return RedirectToAction("HomePage");
            //return RedirectToAction("HomePage", new { username = username });
        }

        public ActionResult Following(String username)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {    
                dynamic mymodel = new ExpandoObject();
                mymodel.user = CRUD.view_user(Session["username"].ToString());
                mymodel.no_of_followings = CRUD.no_of_followings(Session["username"].ToString());
                mymodel.following = CRUD.get_following(Session["username"].ToString());
                Console.Write(mymodel);
                return View(mymodel);
            }
        }

        public ActionResult Followers(String username)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                dynamic mymodel = new ExpandoObject();
                mymodel.user = CRUD.view_user(Session["username"].ToString());
                mymodel.no_of_followers = CRUD.no_of_followers(Session["username"].ToString());
                mymodel.followers = CRUD.get_followers(Session["username"].ToString());
                Console.Write(mymodel);
                return View(mymodel);
            }
        }

        public ActionResult Follow(String username)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                CRUD.ToFollow(Session["username"].ToString(), username);
                return RedirectToAction("Following");
            }
        }

        public ActionResult UnFollow(String username)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                CRUD.ToUnFollow(Session["username"].ToString(), username);
                return RedirectToAction("Following");
            }
        }

        public ActionResult RemoveFollower(String username)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                CRUD.RemoveFollower(Session["username"].ToString(), username);
                return RedirectToAction("Followers");
            }
        }
        public ActionResult Update_Name(String username,String password,String first_name,String last_name)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                if(first_name!="" && last_name!="")
                {
                    CRUD.Update_FirstName(username, first_name, password);
                    CRUD.Update_LastName(username, last_name, password);
                }
                return RedirectToAction("Settings");
            }
        }
        public ActionResult Update_Username(String username, String password, String new_username)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                if(new_username!="")
                {
                    if (CRUD.Update_UserName(username, new_username, password) == 1)
                        return RedirectToAction("LogOut");
                    else
                        return RedirectToAction("Settings");
                }
                else
                    return RedirectToAction("Settings");
            }
        }
        public ActionResult Update_Email(String username, String password, String email)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                CRUD.Update_Email(username, email, password);
                return RedirectToAction("Settings");
            }
        }
        public ActionResult Update_Country(String username, String password, String country)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                CRUD.Update_Country(username, country, password);
                return RedirectToAction("Settings");
            }
        }
        public ActionResult Update_Status(String username, String password, String status)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                CRUD.Update_Status(username, status, password);
                return RedirectToAction("Settings");
            }
        }
        public ActionResult Update_Gender(String username, String password, String gender)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                CRUD.Update_Gender(username, gender, password);
                return RedirectToAction("Settings");
            }
        }
        public ActionResult Update_Password(String username, String new_password, String old_password)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                if(new_password!="")
                {
                    if (CRUD.Update_Password(username, new_password, old_password) == 1)
                        return RedirectToAction("LogOut");
                    else
                        return RedirectToAction("Settings");
                }
                else
                    return RedirectToAction("Settings");
            }
        }
        public ActionResult Update_DP(String username, String password, String DP)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                CRUD.Update_DisplayPic(username, DP, password);
                return RedirectToAction("Settings");
            }
        }
        public ActionResult Update_DOB(String username, String password, String DOB)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                CRUD.Update_DOB(username, DOB, password);
                return RedirectToAction("Settings");
            }
        }
        
        
        public ActionResult likeTweet(int tweetID)
        {
            int result = CRUD.like_a_tweet(tweetID, Session["username"].ToString());
            return RedirectToAction("HomePage");
        }

        public ActionResult dislikeTweet(int tweetID)
        {
            int result = CRUD.dislike_a_tweet(tweetID, Session["username"].ToString());
            return RedirectToAction("HomePage");
        }

        public ActionResult commentTweet()
        {
            return View();
        }
        public ActionResult Store_message(string message)
        {
            string sender = Session["username"].ToString();
            if(Globals.recever == null)
                CRUD.storeMessage(sender, " ", message);
            else
                 CRUD.storeMessage(sender, Globals.recever, message);

            return RedirectToAction("Messages");
        }

        public ActionResult postTweet(string tweet)
        {
            if(tweet != "")
            {
                string[] temp = tweet.Split(' ');
                string[] hashtags = {"","","","","","","","","","", "", "", "", "", "", "", "", "", "", ""};
                int j = 0;
                for(int i =0; i < temp.Length; i++)
                {
                    //string check = temp[i];
                    if(temp[i][0] == '#' && !hashtags.Contains(temp[i]))
                    {
                        hashtags[j] = temp[i];
                        j++;
                    }
                }
                int result = CRUD.postTweet(tweet, Session["username"].ToString(),hashtags);
                if(result > 0)
                {
                    CRUD.addHashtags(result, hashtags);
                }
                
                
            }
            return RedirectToAction("HomePage");
        }

        public ActionResult commentOnTweet(string commentText, int tweetID)
        {
            int result = CRUD.commentOnTweet(tweetID, commentText, Session["username"].ToString());
            return RedirectToAction("HomePage");
        }


        public ActionResult DeleteMyAccount()
        {
        if (Session["username"] == null)
                return View("login");
            else
            {

                CRUD.delete_User(Session["username"].ToString());
                return RedirectToAction("LogOut");
}
        }
        
            









        

        public ActionResult UnFollow_in_explore(String username)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {


                CRUD.ToUnFollow(Session["username"].ToString(), username);
                return RedirectToAction("Explore");
            }
        }
        public ActionResult Follow_in_explore(String username)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                CRUD.ToFollow(Session["username"].ToString(), username);
                return RedirectToAction("Explore");
            }
        }
        
        public ActionResult Follow_in_HomePage(String username)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                CRUD.ToFollow(Session["username"].ToString(), username);
                return RedirectToAction("Homepage");
            }
        }
    }
}
