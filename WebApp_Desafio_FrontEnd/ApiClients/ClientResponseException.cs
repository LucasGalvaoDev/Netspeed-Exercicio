using System;
using System.Net;

namespace WebApp_Desafio_FrontEnd.ApiClients
{
    public class ClientResponseException : Exception
    {
        public HttpStatusCode StatusCode { get; private set; }

        public ClientResponseException() : base()
        {

        }

        public ClientResponseException(string message) : base(message)
        {

        }

        public ClientResponseException(string message, Exception inner) : base(message, inner)
        {

        }

        public ClientResponseException(HttpStatusCode statusCode, string message) : base(message)
        {
            this.StatusCode = statusCode;
        }
    }
}
