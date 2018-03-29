using System;
namespace DjangoBlog.Models.RequestContracts
{
    public class AuthRequest
    {
        public int dev_id { get; set; }
        public string user { get; set; }
        public string password { get; set; }
        public string dev_desc { get; set; }
    }

    public class SignupRequest
    {
        public string name { get; set; }
        public string last { get; set; }
        public string email { get; set; }
        public string password { get; set; }
    }

    public class PostRequest
    {
        public string title { get; set; }
        public string entry { get; set; }
    }

    public class CommentRequest
    {
        public string post { get; set; }
        public string comment { get; set; }
    }
}
