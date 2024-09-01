using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WebApp_Desafio_FrontEnd.ApiClients.Desafio_API;
using WebApp_Desafio_FrontEnd.ViewModels;
using WebApp_Desafio_FrontEnd.ViewModels.Enums;
using AspNetCore.Reporting;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Infrastructure;

namespace WebApp_Desafio_FrontEnd.Controllers
{
    public class ChamadosController : Controller
    {
        private readonly IHostingEnvironment _hostEnvironment;

        public ChamadosController(IHostingEnvironment hostEnvironment)
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
                var chamadosApiClient = new ChamadosApiClient();
                var lstChamados = chamadosApiClient.ChamadosListar();

                var dataTableVM = new DataTableAjaxViewModel()
                {
                    length = lstChamados.Count,
                    data = lstChamados
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
            var chamadoVM = new ChamadoViewModel()
            {
                DataAbertura = DateTime.Now
            };
            ViewData["Title"] = "Cadastrar Novo Chamado";

            try
            {
                var departamentosApiClient = new DepartamentosApiClient();

                ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();
            }
            catch (Exception ex)
            {
                ViewData["Error"] = ex.Message;
            }

            return View("Cadastrar", chamadoVM);
        }

        

        [HttpGet]
        public IActionResult Editar([FromRoute] int id)
        {
            ViewData["Title"] = "Cadastrar Novo Chamado";

            try
            {
                var chamadosApiClient = new ChamadosApiClient();
                var chamadoVM = chamadosApiClient.ChamadoObter(id);

                var departamentosApiClient = new DepartamentosApiClient();
                ViewData["ListaDepartamentos"] = departamentosApiClient.DepartamentosListar();

                return View("Cadastrar", chamadoVM);
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
            string path = Path.Combine(contentRootPath, "wwwroot", "reports", "rptChamados.rdlc");

            LocalReport localReport = new LocalReport(path);

            // Carrega os dados que serão apresentados no relatório
            var chamadosApiClient = new ChamadosApiClient();
            var lstChamados = chamadosApiClient.ChamadosListar();

            localReport.AddDataSource("dsChamados", lstChamados);

            ReportResult reportResult = localReport.Execute(RenderType.Pdf);

            return File(reportResult.MainStream, "application/octet-stream", "rptChamados.pdf");
        }
        #endregion

        #region Métodos Post/Delete
        [HttpPost]
        public IActionResult Cadastrar(ChamadoViewModel chamadoVM)
        {
            try
            {
                // Valida a data de abertura
                if (chamadoVM.DataAbertura < DateTime.Now.Date)
                {
                    ModelState.AddModelError("DataAbertura", "Não é possível adicionar uma data retroativa.");
                }

                if (chamadoVM.IdDepartamento == 0)
                {
                    ModelState.AddModelError("Departamento", "Selecione um departamento");
                }

                // Se houver erros de validação, retorna esses erros
                if (!ModelState.IsValid)
                {
                    return Json(new
                    {
                        success = false,
                        message = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()
                    });
                }

                var chamadosApiClient = new ChamadosApiClient();
                var realizadoComSucesso = chamadosApiClient.ChamadoGravar(chamadoVM);

                if (realizadoComSucesso)
                {
                    return Json(new
                    {
                        success = true,
                        message = "Chamado gravado com sucesso!",
                        redirectTo = Url.Action("Listar", "Chamados") // Redireciona para a ação Listar
                    });
                }
                else
                {
                    throw new ApplicationException("Falha ao cadastrar o Chamado.");
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
                var chamadosApiClient = new ChamadosApiClient();
                var realizadoComSucesso = chamadosApiClient.ChamadoExcluir(id);

                if (realizadoComSucesso)
                    return Ok(new ResponseViewModel(
                                $"Chamado {id} excluído com sucesso!",
                                AlertTypes.success,
                                "Chamados",
                                nameof(Listar)));
                else
                    throw new ApplicationException($"Falha ao excluir o Chamado {id}.");
            }
            catch (Exception ex)
            {
                return BadRequest(new ResponseViewModel(ex));
            }
        }

        #endregion

    }
}
