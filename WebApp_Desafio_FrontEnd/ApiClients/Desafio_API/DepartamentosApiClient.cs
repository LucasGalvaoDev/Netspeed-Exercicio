using Newtonsoft.Json;
using System.Collections.Generic;
using WebApp_Desafio_FrontEnd.ViewModels;

namespace WebApp_Desafio_FrontEnd.ApiClients.Desafio_API
{
    public class DepartamentosApiClient : BaseClient
    {
        private const string tokenAutenticacao = "AEEFC184-9F62-4B3E-BB93-BE42BF0FFA36";

        private const string departamentosListUrl = "api/Departamentos/Listar";
        private const string departamentosObterUrl = "api/Departamentos/Obter";
        private const string departamentosGravarUrl = "api/Departamentos/Gravar";
        private const string departamentosExcluirUrl = "api/Departamentos/Excluir";

        private string desafioApiUrl = "https://localhost:44388/"; // Endereço API IIS-Express

        public DepartamentosApiClient() : base()
        {
            //TODO
        }

        public List<DepartamentoViewModel> DepartamentosListar()
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = default(Dictionary<string, object>); // Não há parâmetros para essa chamada

            var response = base.Get($"{desafioApiUrl}{departamentosListUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<List<DepartamentoViewModel>>(json);
        }

        public DepartamentoViewModel DepartamentoObter(int idDepartamento)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = new Dictionary<string, object>()
            {
                { "idDepartamento", idDepartamento }
            };

            var response = base.Get($"{desafioApiUrl}{departamentosObterUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<DepartamentoViewModel>(json);
        }

        public bool DepartamentoGravar(DepartamentoViewModel departamento)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var response = base.Post($"{desafioApiUrl}{departamentosGravarUrl}", departamento, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<bool>(json);
        }

        public bool DepartamentoExcluir(int idDepartamento)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = new Dictionary<string, object>()
            {
                { "idDepartamento", idDepartamento }
            };

            var response = base.Delete($"{desafioApiUrl}{departamentosExcluirUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<bool>(json);
        }
    }
}
