using Newtonsoft.Json;
using System.Collections.Generic;
using WebApp_Desafio_FrontEnd.ViewModels;

namespace WebApp_Desafio_FrontEnd.ApiClients.Desafio_API
{
    public class ChamadosApiClient : BaseClient
    {
        private const string tokenAutenticacao = "AEEFC184-9F62-4B3E-BB93-BE42BF0FFA36";

        private const string chamadosListUrl = "api/Chamados/Listar";
        private const string chamadosObterUrl = "api/Chamados/Obter";
        private const string chamadosGravarUrl = "api/Chamados/Gravar";
        private const string chamadosExcluirUrl = "api/Chamados/Excluir";

        private string desafioApiUrl = "https://localhost:44388/"; // Endereço API IIS-Express

        public ChamadosApiClient() : base()
        {
            //TODO
        }

        public List<ChamadoViewModel> ChamadosListar()
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = default(Dictionary<string, object>); // Não há parâmetros para essa chamada

            var response = base.Get($"{desafioApiUrl}{chamadosListUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<List<ChamadoViewModel>>(json);
        }

        public ChamadoViewModel ChamadoObter(int idChamado)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = new Dictionary<string, object>()
            {
                { "idChamado", idChamado }
            };

            var response = base.Get($"{desafioApiUrl}{chamadosObterUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<ChamadoViewModel>(json);
        }

        public bool ChamadoGravar(ChamadoViewModel chamado)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var response = base.Post($"{desafioApiUrl}{chamadosGravarUrl}", chamado, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<bool>(json);
        }

        public bool ChamadoExcluir(int idChamado)
        {
            var headers = new Dictionary<string, object>()
            {
                { "TokenAutenticacao", tokenAutenticacao }
            };

            var querys = new Dictionary<string, object>()
            {
                { "idChamado", idChamado }
            };

            var response = base.Delete($"{desafioApiUrl}{chamadosExcluirUrl}", querys, headers);

            base.EnsureSuccessStatusCode(response);

            string json = base.ReadHttpWebResponseMessage(response);

            return JsonConvert.DeserializeObject<bool>(json);
        }

    }
}
