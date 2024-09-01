using AspNetCore.Reporting;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System;
using System.IO;
using System.Linq;
using WebApp_Desafio_FrontEnd.ApiClients.Desafio_API;
using WebApp_Desafio_FrontEnd.ViewModels;
using WebApp_Desafio_FrontEnd.ViewModels.Enums;

namespace WebApp_Desafio_FrontEnd.Controllers
{
    public class DepartamentosController : Controller
    {

        private readonly IHostingEnvironment _hostEnvironment;

        public DepartamentosController(IHostingEnvironment hostEnvironment)
        {
            _hostEnvironment = hostEnvironment;
        }

        #region Métodos Get
        [HttpGet]
        public IActionResult Index()
        {
            return RedirectToAction(nameof(Listar));
        }

        [HttpGet]
        public IActionResult Listar()
        {
            // Busca de dados está na Action Datatable()
            return View();
        }

        [HttpGet]
        public IActionResult Datatable()
        {
            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var lstDepartamentos = departamentosApiClient.DepartamentosListar();

                var dataTableVM = new DataTableAjaxViewModel()
                {
                    length = lstDepartamentos.Count,
                    data = lstDepartamentos
                };

                return Ok(dataTableVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            ViewData["Title"] = "Cadastrar Novo Departamento";

            try
            {
                var departamentosApiClient = new DepartamentosApiClient();

                ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            return View("Cadastrar");
        }

        [HttpGet]
        public IActionResult Editar([FromRoute] int id)
        {
            ViewData["Title"] = "Cadastrar Novo Departamento";

            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var departamentoVM = departamentosApiClient.DepartamentoObter(id);


                return View("Cadastrar", departamentoVM);
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        [HttpGet]
        public IActionResult Report()
        {
            string mimeType = string.Empty;
            int extension = 1;
            string contentRootPath = _hostEnvironment.ContentRootPath;
            string path = Path.Combine(contentRootPath, "wwwroot", "reports", "rptDepartamentos.rdlc");

            LocalReport localReport = new LocalReport(path);

            // Carrega os dados que serão apresentados no relatório
            var departamentosApiClient = new DepartamentosApiClient();
            var lstDepartamentos = departamentosApiClient.DepartamentosListar();

            localReport.AddDataSource("dsDepartamentos", lstDepartamentos);

            // Renderiza o relatório em PDF
            ReportResult reportResult = localReport.Execute(RenderType.Pdf);

            //return File(reportResult.MainStream, "application/pdf");
            return File(reportResult.MainStream, "application/octet-stream", "rptDepartamentos.pdf");
        }

        #endregion

        #region Métodos Post/Delete
        [HttpPost]
        public IActionResult Cadastrar(DepartamentoViewModel departamentoVM)
        {
            try
            {
                ModelState.Remove("ID");

                if (departamentoVM.Descricao == null || departamentoVM.Descricao.Trim().Length == 0)
                {
                    ModelState.AddModelError("Nome", "O nome do departamento é obrigatório.");
                }

                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        success = false,
                        message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                    });
                }

                var departamentosApiClient = new DepartamentosApiClient();
                var realizadoComSucesso = departamentosApiClient.DepartamentoGravar(departamentoVM);

                if (realizadoComSucesso)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Departamento gravado com sucesso!",
                        redirectTo = Url.Action("Listar", "Departamentos") // Ajuste o nome do controller se necessário
                    });
                }
                else
                {
                    throw new ApplicationException("Falha ao cadastrar o Departamento.");
                }
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    success = false,
                    message = ex.Message
                });
            }

        }

        [HttpDelete]
        public IActionResult Excluir([FromRoute] int id)
        {
            try
            {
                var departamentosApiClient = new DepartamentosApiClient();
                var realizadoComSucesso = departamentosApiClient.DepartamentoExcluir(id);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Departamento {id} excluído com sucesso!",
                                AlertTypes.success,
                                "Departamentos",
                                nameof(Listar)));
                else
                    throw new ApplicationException($"Falha ao excluir o Departamento {id}.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }
        #endregion
    }
}
