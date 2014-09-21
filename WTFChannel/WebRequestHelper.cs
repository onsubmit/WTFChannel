//-----------------------------------------------------------------------
// <copyright file="WebRequestHelper.cs" company="Andy Young">
//     Copyright (c) Andy Young. All rights reserved.
// </copyright>
//-----------------------------------------------------------------------
namespace WTFChannel
{
    using System.IO;
    using System.Net;

    /// <summary>
    /// Web request helper
    /// </summary>
    public class WebRequestHelper
    {
        /// <summary>
        /// Gets the response
        /// </summary>
        /// <param name="uri">URI to request</param>
        /// <param name="response">Returned response</param>
        /// <returns>Response string</returns>
        public string GetResponse(string uri, out HttpWebResponse response)
        {
            return this.GetFinalResponse(uri, out response);
        }

        /// <summary>
        /// Creates a web request
        /// </summary>
        /// <param name="uri">URI to request</param>
        /// <param name="method">HTTP method</param>
        /// <returns>Web request</returns>
        private HttpWebRequest CreateWebRequest(string uri, string method = "GET")
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.KeepAlive = false;
            request.Method = method;
            return request;
        }

        /// <summary>
        /// Gets the response from the web request
        /// </summary>
        /// <param name="uri">URI to request</param>
        /// <param name="response">Returned response</param>
        /// <returns>Response string</returns>
        private string GetFinalResponse(string uri, out HttpWebResponse response)
        {
            HttpWebRequest request = this.CreateWebRequest(uri, "GET");
            try
            {
                response = (HttpWebResponse)request.GetResponse();
            }
            catch (WebException wex)
            {
                response = (HttpWebResponse)wex.Response;
                return wex.Message;
            }

            StreamReader stream = new StreamReader(response.GetResponseStream());
            string responseString = stream.ReadToEnd();
            stream.Close();
            response.Close();
            return responseString;
        }
    }
}