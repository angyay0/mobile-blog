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

            if (method == HttpMethod.Post || method == HttpMethod.Put || method == new HttpMethod("PATCH"))
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

        private void ExtractData<T>(HttpResponseMessage raw,T outParam)
        {
            string jsonResponse;

            switch(raw.StatusCode)
            {
                case HttpStatusCode.OK:
                    //OK Handler
                    jsonResponse = raw.Content.ReadAsStringAsync().Result;
                    Constants.AppLogger(jsonResponse);
                    outParam = JsonConvert.DeserializeObject<T>(jsonResponse);
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
                    outParam = JsonConvert.DeserializeObject<T>(jsonResponse);
                    break;
            }
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
            var request = GenerateRequest(new Uri(SigngUpEndpoint), HttpMethod.Post, null, data);

            var _raw = await sender.SendAsync(request);
            ExtractData(_raw,response);

            return response;
        }

        public async Task<APIResponse<AuthCredentials>> Authenticate(AuthRequest data)
        {
            APIResponse<AuthCredentials> response = null;
            var request = GenerateRequest(new Uri(AuthEndpoint), HttpMethod.Post, null, data);

            var _raw = await sender.SendAsync(request);
            ExtractData(_raw, response);

            //Credentials Obtention per OAuth Imp
            credentials.Add("Key", response.data.api);
            credentials.Add("AuthKey", response.data.auth);

            return response;
        }

        public async Task<List<Post>> FetchPosts()
        {
            APIResponse<List<Post>> response = null;
            var request = GenerateRequest(new Uri(PostEndpoint), HttpMethod.Get, credentials, null);

            var _raw = await sender.SendAsync(request);
            ExtractData(_raw, response);

            return response.data;
        }

        public async Task<Post> FetchPostById(string key)
        {
            APIResponse<Post> response = null;
            var request = GenerateRequest(new Uri(string.Format(PostRetrieveEndpoint,key)), HttpMethod.Get, credentials, null);

            var _raw = await sender.SendAsync(request);
            ExtractData(_raw, response);

            return response.data;
        }

        public async Task<APIResponse<Post>> CreateAPost(PostRequest data)
        {
            APIResponse<Post> response = null;
            var request = GenerateRequest(new Uri(PostEndpoint), HttpMethod.Post, credentials, data);

            var _raw = await sender.SendAsync(request);
            ExtractData(_raw, response);

            return response;
        }

        public async Task<int> CreateAComment(CommentRequest data)
        {
            APIResponse<Post> response = null;
            var request = GenerateRequest(new Uri(CommentEndpoint), HttpMethod.Post, credentials, data);

            var _raw = await sender.SendAsync(request);
            ExtractData(_raw, response);

            return response.code;
        }
    }
}
