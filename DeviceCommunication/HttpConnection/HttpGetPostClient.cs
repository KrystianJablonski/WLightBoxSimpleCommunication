using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeviceCommunication.HttpConnection
{
    /// <summary>
    /// Implementation of <seealso cref="IGetPostClient"/> interface.
    /// Redirects Get and Post methods to <seealso cref="HttpClient"/>.
    /// </summary>
    public class HttpGetPostClient : IGetPostClient
    {
        private HttpClient _httpClient = new HttpClient();
        public Task<HttpResponseMessage> GetAsync(string requestUri) => _httpClient.GetAsync(requestUri);

        public Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content) => _httpClient.PostAsync(requestUri, content);
    }
}
