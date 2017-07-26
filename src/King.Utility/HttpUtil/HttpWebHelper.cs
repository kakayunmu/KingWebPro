using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace King.Utility.HttpUtil
{
    public class HttpWebHelper
    {
        public static async Task<WebResponse> Get(string url, Dictionary<string, string> headerParame = null, int timeout = 15)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "GET";
            request.Proxy = null;
            request.ContinueTimeout = timeout * 1000;
            if (headerParame != null && headerParame.Keys.Count > 0)
            {
                foreach (var item in headerParame.Keys)
                {
                    request.Headers[item] = headerParame[item];
                }
            }
            return await request.GetResponseAsync();
        }

        public static async Task<WebResponse> PostWithJson(string url, object postData = null, Dictionary<string, string> headerParame = null, int timeout = 15)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.Proxy = null;
            request.ContinueTimeout = timeout * 1000;
            request.ContentType = "application/json";
            if (headerParame != null && headerParame.Keys.Count > 0)
            {
                foreach (var item in headerParame.Keys)
                {
                    request.Headers[item] = headerParame[item];
                }
            }
            if (postData != null)
            {
                byte[] data = Encoding.UTF8.GetBytes(Newtonsoft.Json.JsonConvert.SerializeObject(postData));
                using (Stream stream = await request.GetRequestStreamAsync())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return await request.GetResponseAsync();
        }

        public static async Task<WebResponse> Post(string url, Dictionary<string, string> postData = null, Dictionary<string, string> headerParame = null, int timeout = 15)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentException("URL");
            }
            HttpWebRequest request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            request.Proxy = null;
            request.ContinueTimeout = timeout * 1000;
            request.ContentType = "application/x-www-form-urlencoded";
            if (headerParame != null && headerParame.Keys.Count > 0)
            {
                foreach (var item in headerParame.Keys)
                {
                    request.Headers[item] = headerParame[item];
                }
            }
            if (postData != null && postData.Keys.Count > 0)
            {
                StringBuilder buffer = new StringBuilder();
                foreach (var key in postData.Keys)
                {
                    buffer.AppendFormat("&{0}={1}", key, postData[key]);
                }

                byte[] data = Encoding.UTF8.GetBytes(buffer.ToString().Trim('&'));
                using (Stream stream = await request.GetRequestStreamAsync())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            return await request.GetResponseAsync();
        }
    }


}
