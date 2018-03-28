using System;
using System.Collections.Generic;

namespace DjangoBlog.Models.ResponseContracts
{
    public class APIResponse<T>
    {
        public string message { get; set; }
        public int code { get; set; }
        public T data { get; set; }
    }

    public class AuthCredentials
    {
        public string api { get; set; }
        public string auth { get; set; }
        public User user { get; set; }
    }

    public class User
    {
        public string name { get; set; }
        public string last { get; set; }
        public string email { get; set; }
    }

    public class Post
    {
        public string author { get; set; }
        public string title { get; set; }
        public string entry { get; set; }
        public string id { get; set; }
        public List<Comment> comments { get; set; }
        public DateTime edited { get; set; }
        public DateTime created { get; set; }
    }

    public class Comment
    {
        public string comment { get; set; }
        public string author { get; set; }
        public DateTime date { get; set; }
    }
}
