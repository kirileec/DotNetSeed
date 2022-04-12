using Newtonsoft.Json;
using RestSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{

    public interface ISingleClient
    {

    }

    public class SimpleHttpClient: ISingleClient
    {
        private RestClient _client = null;

        private static SimpleHttpClient _instance = null;
        public static SimpleHttpClient Instance { get
            {
                if (_instance == null)
                {
                    var options = new RestClientOptions()
                    {
                        ThrowOnAnyError = true,
                        Timeout = 10000
                    };
                    _instance = new SimpleHttpClient { _client = new RestClient(options).UseJson() };
                }
                return _instance;
            } 
        }

        private SimpleHttpClient(){}

        public E Get<E>(string url, Func<ParametersCollection> func = null, ICollection<KeyValuePair<string, string>> headers = null)
        {
            var request = new RestRequest(url);
            if (headers != null)
            {
                request.AddHeaders(headers);
            }
            if (func!=null)
            {
                var ps = func.Invoke();
                if (ps.Count > 0)
                {
                    ps.ToList().ForEach(p =>
                    {
                        request.AddQueryParameter(p.Name, p.Value.ToString());
                    });
                }
            }
            
           
            return _client.GetAsync<E>(request).Result;
        }

        public E Post<E>(string url, ICollection<KeyValuePair<string, string>> headers = null,object ? body = null )
        {
            var request = new RestRequest(url);
            request.RequestFormat = DataFormat.Json;
            request.Method = Method.Post;
            request.AddHeader("Accept", "application/json");
            
            if (headers!=null)
            {
                request.AddHeaders(headers);
            }

            if (body!=null)
            {
                request.AddJsonBody(body);
            }

            var resp = _client.PostAsync<E>(request);
            try
            {
                Console.WriteLine(resp.GetAwaiter().GetResult());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                //throw;
            }

            return resp.Result;
        }
        public E Post<T, E>(string url, T body, ICollection<KeyValuePair<string, string>> headers = null) where T : class
        {
            var request = new RestRequest(url);
            request.AddHeader("Content-Type", "application/json");
            if (headers != null)
            {
                request.AddHeaders(headers);
            }
            request.Method = Method.Post;
            request.AddJsonBody(body);
            
            return _client.ExecuteAsync<E>(request).Result.Data;
        }
        public Task<E> PostAsync<T, E>(string url, T body, ICollection<KeyValuePair<string, string>> headers = null) where T : class
        {
            var request = new RestRequest(url);
            request.AddHeader("Content-Type", "application/json");
            if (headers != null)
            {
                request.AddHeaders(headers);
            }
            request.AddJsonBody(body);
            return _client.PostAsync<E>(request);
        }

         public Task<E> PostAsync<E>(string url, object body, ICollection<KeyValuePair<string, string>> headers = null)
        {
            var request = new RestRequest(url);
            request.AddHeader("Content-Type", "application/json");
            if (headers != null)
            {
                request.AddHeaders(headers);
            }
            request.AddJsonBody(body);
            return _client.PostAsync<E>(request);
        }

        public Task<E> PutAsync<E>(string url, object body, ICollection<KeyValuePair<string, string>> headers = null)
        {
            var request = new RestRequest(url);
            request.Method = Method.Put;
            
            if (headers != null)
            {
                request.AddHeaders(headers);
            }
            request.AddJsonBody(body);
            return _client.PutAsync<E>(request);
        }



    }
}
