using System;
using System.Collections.Generic;
using WebApp_Desafio_BackEnd.DataAccess;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.Business
{
    public class ChamadosBLL
    {
        private ChamadosDAL dal = new ChamadosDAL();

        public IEnumerable<Chamado> ListarChamados()
        {
            return dal.ListarChamados();
        }

        public Chamado ObterChamado(int idChamado)
        {
            return dal.ObterChamado(idChamado);
        }

        public bool GravarChamado(int ID, string Assunto, string Solicitante, int IdDepartamento, DateTime DataAbertura)
        {
            return dal.GravarChamado(ID, Assunto, Solicitante, IdDepartamento, DataAbertura);
        }

        public bool ExcluirChamado(int idChamado)
        {
            return dal.ExcluirChamado(idChamado);
        }
    }
}
