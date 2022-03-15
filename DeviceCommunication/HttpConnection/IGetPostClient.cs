using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace DeviceCommunication.HttpConnection
{
    /// <summary>
    /// Interface with Get and Post request functionality.
    /// Allows for dependency injection in the <seealso cref="HttpDeviceConnection"/> class.
    /// </summary>
    public interface IGetPostClient
    {
        /// <summary>
        /// Send get request to <paramref name="requestUri"/>.
        /// </summary>
        /// <returns>Get response</returns>
        Task<HttpResponseMessage> GetAsync(string requestUri);
        /// <summary>
        /// Send post request to <paramref name="requestUri"/> with given <paramref name="content"/>
        /// </summary>
        /// <returns>Post response</returns>
        Task<HttpResponseMessage> PostAsync(string requestUri, HttpContent content);
    }
}
