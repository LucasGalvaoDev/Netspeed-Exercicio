using System;
using WebApp_Desafio_API.ViewModels.Enums;

namespace WebApp_Desafio_API.ViewModels
{
    /// <summary>
    /// 
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Get or Set Message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Get or Set Message
        /// </summary>
        public string Validation { get; set; }

        /// <summary>
        /// Get or Set PropertyName
        /// </summary>
        public string PropertyName { get; set; }

        /// <summary>
        /// Get or Set StatusCode
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Get or Set Type
        /// </summary>
        public AlertTypes Type { get; set; }
    }
}