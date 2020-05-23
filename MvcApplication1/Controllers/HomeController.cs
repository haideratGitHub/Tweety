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

        public ActionResult Login()
        {
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
        public ActionResult LogOut()
        {
            Session["username"] = null;
            return View("Login");
        }
        public ActionResult Messages()
        {
            return View("Messages");
        }
        public ActionResult Settings()
        {
            return View("Settings");
        }
        public ActionResult Notifications()
        {
            return View("Notifications");
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
            if (Session["username"] == null)
                return View("login");
            else
            {
                User users = CRUD.view_user(Session["username"].ToString());
                List<User> People_U_Should_Follow_list = CRUD.People_U_Should_Follow(Session["username"].ToString());
                List<hashtag_trending> trendingHashtags = CRUD.trending_hashtag();

                dynamic model = new ExpandoObject();
                model.User = users;
                model.Suggested_people = People_U_Should_Follow_list;
                model.trending_hashtags = trendingHashtags;
              
                return View(model);
            }
        }

        public ActionResult ProfilePage(String username)
        {
            if (Session["username"] == null)
                return View("login");
            else
            {
                //User users = CRUD.view_user(Session["username"].ToString());
                dynamic mymodel = new ExpandoObject();
                mymodel.user = CRUD.view_user(Session["username"].ToString());
                mymodel.no_of_followers = CRUD.no_of_followers(Session["username"].ToString());
                mymodel.no_of_followings = CRUD.no_of_followings(Session["username"].ToString());
                //if (users == null)
                //{
                //    RedirectToAction("Login");
                //}
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
    }
}
