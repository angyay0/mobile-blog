using System;
using DjangoBlog.Models.ResponseContracts;

namespace DjangoBlog.Helper
{
    public class Constants
    {
        #if DEBUG
        public const bool _DEV = true;
        public const string BASE_URL = "http://192.168.73.26:8000/api/";
#else
        public const bool _DEV = false;
        public const string BASE_URL = "http://mydomain.somewhere.else/api/";
#endif

        public static User user;

        public static void AppLogger(params string[] output)
        {
            if (_DEV)
            {
                Console.WriteLine("** Django Blog Xamarin Output: Start **");
                foreach(string s in output)
                {
                    Console.WriteLine(s);
                }
                Console.WriteLine("** Django Blog Xamarin Output: Start **");
            }
        }
    }
}
