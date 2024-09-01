using Microsoft.AspNetCore.Mvc;
using System;
using WebApp_Desafio_API.ViewModels;
using WebApp_Desafio_API.ViewModels.Enums;

namespace WebApp_Desafio_API.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class ControllerExtensions
    {
        /// <summary>
        /// Retorna um Objeto completo com o StatusCode
        /// </summary>
        /// <param name="controllerBase"></param>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static ObjectResult ExceptionProcess(this ControllerBase controllerBase, Exception ex)
        {
            return ExceptionProcess(controllerBase, ex, AlertTypes.warning);
        }

        /// <summary>
        /// Retorna um Objeto completo com o StatusCode
        /// </summary>
        /// <param name="controllerBase"></param>
        /// <param name="ex"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static ObjectResult ExceptionProcess(this ControllerBase controllerBase, Exception ex, AlertTypes type)
        {
            ErrorViewModel errorViewModel = new ErrorViewModel() { Message = ex.Message, StatusCode = 500 };

            if (ex is ArgumentException aex)
            {
                errorViewModel.StatusCode = 400;
            }
            else if (ex is ApplicationException apex)
            {
                errorViewModel.StatusCode = 422;
            }
            //else if (ex is ResponseException rex)
            //{
            //    errorViewModel.StatusCode = 422;
            //    errorViewModel.PropertyName = rex.PropertyName;
            //    errorViewModel.Validation = rex.Validation;
            //}

            errorViewModel.Type = type;

            return controllerBase.StatusCode(errorViewModel.StatusCode, errorViewModel);
        }
    }
}