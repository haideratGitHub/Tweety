using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Models
{
    public class Tweet
    {
        public string tweetID;
        public string userID;
        public string tweet;
        public string date;
        public string time;

        public int no_of_comments;
        public int no_of_likes;
        public int no_of_dislikes;

        public List<Comment> comments;
        public List<User> likers;
        public List<User> dislikers;

        //JUST FOR MODALS USE
        public int modelID1;
        public int modelID2;
    }
}
