namespace HackerNews
{
    using System;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// 
    /// </summary>
    public class RestClient
    {
        /// <summary>
        /// 
        /// </summary>
        private static readonly Lazy<RestClient> LazyInstance = new Lazy<RestClient>(() => new RestClient());
        
        /// <summary>
        /// 
        /// </summary>
        public static RestClient Instance
        {
            get
            {
                return LazyInstance.Value;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Get(string uri)
        {
            using (var httpClient = new HttpClient())
            {
                try
                {
                    return await httpClient.GetAsync(uri);
                }
                catch (Exception ex)
                {
                    //Log error
                    throw ex;
                }
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="uri"></param>
        /// <param name="payload"></param>
        /// <returns></returns>
        public async Task<HttpResponseMessage> Post(string uri, string payload)
        {
            using(var httpClient = new HttpClient())
            {
                try
                {
                    HttpContent httpContent = new StringContent(payload, Encoding.UTF8, "application/json");

                    return await httpClient.PostAsync(uri, httpContent);
                }
                catch(Exception ex)
                {
                    //Log error
                    throw ex;
                }
            }
        }
    }
}
