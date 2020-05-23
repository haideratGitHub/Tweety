using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MvcApplication1.Models
{
    public class Comment
    {
        public string commentID;
        public string tweetID;
        public string commenterID;
        public string comment;
        public string date;
        public string time;

        public User commenter;
    }
}
