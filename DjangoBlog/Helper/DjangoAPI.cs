using System;
using System.Net;
using System.Text;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Collections.Generic;

using Newtonsoft.Json;
using System.Threading.Tasks;
using DjangoBlog.Models.RequestContracts;
using DjangoBlog.Models.ResponseContracts;

namespace DjangoBlog.Helper
{
    public class DjangoAPI
    {
        /* Inner Members */
        Dictionary<string, string> credentials;
        static DjangoAPI instance;
        HttpClient sender;
        /* Inner Members */

        /* Endpoints */
        string SigngUpEndpoint = "signup/";
        string AuthEndpoint = "authenticate/";
        string PostEndpoint = "post/";
        string PostRetrieveEndpoint = "post/?post={0}";
        string CommentEndpoint = "post/comment/";
        /* Endpoints */

        private DjangoAPI()
        {
            sender = new HttpClient();
            credentials = new Dictionary<string, string>();
        }

        private HttpRequestMessage GenerateRequest(Uri uri,HttpMethod method,Dictionary<string,string> headers, object data)
        {
            HttpRequestMessage request = new HttpRequestMessage()
            {
                RequestUri = uri,
                Method = method
            };

            if (method == HttpMethod.Post)
            {
                request.Content = new StringContent(JsonConvert.SerializeObject(data),
                                                    Encoding.UTF8, "application/json");
            }

            if (headers != null)
            {
                request.Headers.Accept.Clear();
                request.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                headers.ToList().ForEach(header =>
                {
                    request.Headers.Add(header.Key, header.Value);
                });
            }

            return request;
        }

        private APIResponse<T> ExtractData<T>(HttpResponseMessage raw)
        {
            string jsonResponse;
            APIResponse<T> outParam;

            switch(raw.StatusCode)
            {
                case HttpStatusCode.OK:
                    //OK Handler
                    jsonResponse = raw.Content.ReadAsStringAsync().Result;
                    Constants.AppLogger(jsonResponse);
                    outParam = JsonConvert.DeserializeObject<APIResponse<T>>(jsonResponse);
                    break;
                /*case HttpStatusCode.Unauthorized:
                case HttpStatusCode.Forbidden:
                    //No Credentials Valid
                    break;
                case HttpStatusCode.BadRequest:
                case HttpStatusCode.NotAcceptable:
                    //Not Acceptable Group
                    break;
                case HttpStatusCode.InternalServerError:
                    //Ups Server is broken
                    break;*/
                default:
                    //Nothing to do
                    jsonResponse = raw.Content.ReadAsStringAsync().Result;
                    Constants.AppLogger(jsonResponse);
                    outParam = JsonConvert.DeserializeObject<APIResponse<T>>(jsonResponse);
                    break;
            }

            return outParam;
        }

        public static DjangoAPI Instance()
        {
            if (instance == null)
                instance = new DjangoAPI();

            return instance;
        }

        public async Task<APIResponse<string>> SignupRequest(SignupRequest data)
        {
            APIResponse<string> response = null;
            var request = GenerateRequest(new Uri(Constants.BASE_URL+SigngUpEndpoint), HttpMethod.Post, null, data);

            var _raw = await sender.SendAsync(request);
            response = ExtractData<string>(_raw);

            return response;
        }

        public async Task<APIResponse<AuthCredentials>> Authenticate(AuthRequest data)
        {
            APIResponse<AuthCredentials> response = null;
            var request = GenerateRequest(new Uri(Constants.BASE_URL+AuthEndpoint), HttpMethod.Post, null, data);

            var _raw = await sender.SendAsync(request);
            response = ExtractData<AuthCredentials>(_raw);

            //Credentials Obtention per OAuth Imp
            credentials.Add("Key", response.data.api);
            credentials.Add("AuthKey", response.data.auth);

            return response;
        }

        public async Task<List<Post>> FetchPosts()
        {
            APIResponse<List<Post>> response = null;
            var request = GenerateRequest(new Uri(Constants.BASE_URL+PostEndpoint), HttpMethod.Get, credentials, null);

            var _raw = await sender.SendAsync(request);
            response = ExtractData<List<Post>>(_raw);

            return response.data;
        }

        public async Task<Post> FetchPostById(string key)
        {
            APIResponse<Post> response = null;
            var request = GenerateRequest(new Uri(Constants.BASE_URL+string.Format(PostRetrieveEndpoint,key)), HttpMethod.Get, credentials, null);

            var _raw = await sender.SendAsync(request);
            response = ExtractData<Post>(_raw);

            return response.data;
        }

        public async Task<APIResponse<Post>> CreateAPost(PostRequest data)
        {
            APIResponse<Post> response = null;
            var request = GenerateRequest(new Uri(Constants.BASE_URL+PostEndpoint), HttpMethod.Post, credentials, data);

            var _raw = await sender.SendAsync(request);
            response = ExtractData<Post>(_raw);

            return response;
        }

        public async Task<int> CreateAComment(CommentRequest data)
        {
            APIResponse<object> response = null;
            var request = GenerateRequest(new Uri(Constants.BASE_URL+CommentEndpoint), HttpMethod.Post, credentials, data);

            var _raw = await sender.SendAsync(request);
            response = ExtractData<object>(_raw);

            return response.code;
        }
    }
}
