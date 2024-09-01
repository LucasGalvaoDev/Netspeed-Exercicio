using System;

namespace WebApp_Desafio_API.ViewModels
{
    /// <summary>
    /// Solicitação da chamada
    /// </summary>
    public class DepartamentoRequest
    {
        /// <summary>
        /// ID do Departamento
        /// </summary>
        public int id { get; set; }

        /// <summary>
        /// Descrição do Departamento
        /// </summary>
        public string descricao { get; set; }
    }
}
