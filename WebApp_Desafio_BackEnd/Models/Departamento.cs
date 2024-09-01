using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebApp_Desafio_BackEnd.Models
{
    [Serializable]
    public class Departamento
    {
        public static readonly Departamento Empty;

        [Key]
        public int ID { get; set; }

        [Required(ErrorMessage = "A Descricao é obrigatória")]
        public string Descricao { get; set; }
    }
}
