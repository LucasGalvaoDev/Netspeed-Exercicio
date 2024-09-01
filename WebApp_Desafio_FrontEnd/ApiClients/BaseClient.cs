using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_Desafio_FrontEnd.ApiClients
{
    public abstract class BaseClient
    {
        protected void EnsureSuccessStatusCode(HttpWebResponse httpResponseMessage)
        {
            if ((int)httpResponseMessage.StatusCode >= 400)
                throw new ClientResponseException(httpResponseMessage.StatusCode, this.ReadHttpWebResponseMessage(httpResponseMessage));
        }

        protected string ReadHttpWebResponseMessage(HttpWebResponse httpResponseMessage)
        {
            string message = string.Empty;
            using (var reader = new System.IO.StreamReader(httpResponseMessage.GetResponseStream()))
                message = reader.ReadToEnd();

            return message;
        }

        private WebClient CreateWebClient(ref string url, Dictionary<string, object> queries = null, Dictionary<string, object> headers = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL não pode ser vazia ou nula.");

            if (url.Contains('?'))
                throw new ArgumentException("URL já contém parâmetros. (?)");

            WebClient cliente = new WebClient();

            if (queries != null)
            {
                url += "?";
                foreach (var query in queries)
                {
                    url += $"{query.Key}={(query.Value ?? string.Empty)}&";
                }
            }

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    cliente.Headers.Add(header.Key, header.Value.ToString());
                }
            }

            return cliente;
        }

        private HttpWebRequest CreateWebRequest(ref string url, Dictionary<string, object> queries = null, Dictionary<string, object> headers = null)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentException("URL não pode ser vazia ou nula.");

            if (url.Contains('?'))
                throw new ArgumentException("URL já contém parâmetros. (?)");

            if (queries != null)
            {
                url += "?";

                foreach (var query in queries)
                {
                    url += $"{query.Key}={(query.Value ?? string.Empty)}&";
                }
            }

            var webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.ContentType = "application/json";
            webRequest.Accept = "application/json";

            if (headers != null)
            {
                foreach (var header in headers)
                {
                    webRequest.Headers.Add(header.Key, header.Value.ToString());
                }
            }

            return webRequest;
        }

        protected HttpWebResponse Get(string url, Dictionary<string, object> queries = null, Dictionary<string, object> headers = null)
        {
            try
            {
                using (WebClient cliente = this.CreateWebClient(ref url, queries, headers))
                {
                    var webRequest = (HttpWebRequest)WebRequest.Create(url);

                    if (headers != null)
                    {
                        foreach (var header in headers)
                        {
                            if (header.Value != null)
                                webRequest.Headers.Add(header.Key, header.Value.ToString());
                        }
                    }

                    return (HttpWebResponse)webRequest.GetResponse();
                }
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.ProtocolError)
            {
                WebResponse errResp = ((WebException)e).Response;

                string responseMessage = string.Empty;

                using (Stream stream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    responseMessage = reader.ReadToEnd();
                }

                throw new Exception(responseMessage);
            }
            
        }

        protected HttpWebResponse Post<T>(string url, T body, Dictionary<string, object> queries = null, Dictionary<string, object> headers = null)
        {
            try
            {
                string json = JsonConvert.SerializeObject(body);
                byte[] byteArray = Encoding.UTF8.GetBytes(json);

                var webRequest = this.CreateWebRequest(ref url, queries, headers);
                webRequest.Method = WebRequestMethods.Http.Post;
                webRequest.ContentLength = byteArray.Length;

                using (Stream stream = webRequest.GetRequestStream())
                {
                    stream.Write(byteArray, 0, byteArray.Length);
                }

                return (HttpWebResponse)webRequest.GetResponse();
            }
            catch (WebException e) when (e.Status == WebExceptionStatus.ProtocolError)
            {
                WebResponse errResp = ((WebException)e).Response;

                string responseMessage = string.Empty;

                using (Stream stream = errResp.GetResponseStream())
                {
                    StreamReader reader = new StreamReader(stream);
                    responseMessage = reader.ReadToEnd();
                }

                throw new Exception(responseMessage);
            }
        }

        protected HttpWebResponse Put<T>(string url, T body, Dictionary<string, object> queries = null, Dictionary<string, object> headers = null)
        {
            string json = JsonConvert.SerializeObject(body);
            byte[] byteArray = Encoding.UTF8.GetBytes(json);

            var webRequest = this.CreateWebRequest(ref url, queries, headers);
            webRequest.Method = WebRequestMethods.Http.Put;
            webRequest.ContentLength = byteArray.Length;

            using (Stream stream = webRequest.GetRequestStream())
            {
                stream.Write(byteArray, 0, byteArray.Length);
            }

            return (HttpWebResponse)webRequest.GetResponse();
        }

        protected HttpWebResponse Delete(string url, Dictionary<string, object> queries = null, Dictionary<string, object> headers = null)
        {
            var webRequest = this.CreateWebRequest(ref url, queries, headers);
            webRequest.Method = "DELETE";

            return (HttpWebResponse)webRequest.GetResponse();
        }
    }
}
