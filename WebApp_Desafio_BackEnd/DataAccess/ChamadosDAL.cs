using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebApp_Desafio_BackEnd.Models;

namespace WebApp_Desafio_BackEnd.DataAccess
{
    public class ChamadosDAL : BaseDAL
    {
        private const string ANSI_DATE_FORMAT = "yyyy-MM-dd";

        public IEnumerable<Chamado> ListarChamados()
        {
            IList<Chamado> lstChamados = new List<Chamado>();

            DataTable dtChamados = new DataTable();

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {

                    dbCommand.CommandText = 
                        "SELECT chamados.ID, " + 
                        "       Assunto, " +
                        "       Solicitante, " +
                        "       IdDepartamento, " +
                        "       departamentos.Descricao AS Departamento, " + 
                        "       DataAbertura " + 
                        "FROM chamados " + 
                        "INNER JOIN departamentos " +
                        "   ON chamados.IdDepartamento = departamentos.ID ";

                    dbConnection.Open();

                    using (SQLiteDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        var chamado = Chamado.Empty;

                        while (dataReader.Read())
                        {
                            chamado = new Chamado();

                            if (!dataReader.IsDBNull(0))
                                chamado.ID = dataReader.GetInt32(0);
                            if (!dataReader.IsDBNull(1))
                                chamado.Assunto = dataReader.GetString(1);
                            if (!dataReader.IsDBNull(2))
                                chamado.Solicitante = dataReader.GetString(2);
                            if (!dataReader.IsDBNull(3))
                                chamado.IdDepartamento = dataReader.GetInt32(3);
                            if (!dataReader.IsDBNull(4))
                                chamado.Departamento = dataReader.GetString(4);
                            if (!dataReader.IsDBNull(5))
                                chamado.DataAbertura = DateTime.Parse(dataReader.GetString(5));

                            lstChamados.Add(chamado);
                        }
                        dataReader.Close();
                    }
                    dbConnection.Close();
                }

            }

            return lstChamados;
        }

        public Chamado ObterChamado(int idChamado)
        {
            var chamado = Chamado.Empty;

            DataTable dtChamados = new DataTable();

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText =
                        "SELECT chamados.ID, " +
                        "       Assunto, " +
                        "       Solicitante, " +
                        "       IdDepartamento, " +
                        "       departamentos.Descricao AS Departamento, " +
                        "       DataAbertura " +
                        "FROM chamados " +
                        "INNER JOIN departamentos " +
                        "   ON chamados.IdDepartamento = departamentos.ID " +
                        $"WHERE chamados.ID = {idChamado}";

                    dbConnection.Open();

                    using (SQLiteDataReader dataReader = dbCommand.ExecuteReader())
                    {
                        if (dataReader.Read())
                        {
                            chamado = new Chamado();

                            if (!dataReader.IsDBNull(0))
                                chamado.ID = dataReader.GetInt32(0);
                            if (!dataReader.IsDBNull(1))
                                chamado.Assunto = dataReader.GetString(1);
                            if (!dataReader.IsDBNull(2))
                                chamado.Solicitante = dataReader.GetString(2);
                            if (!dataReader.IsDBNull(3))
                                chamado.IdDepartamento = dataReader.GetInt32(3);
                            if (!dataReader.IsDBNull(4))
                                chamado.Departamento = dataReader.GetString(4);
                            if (!dataReader.IsDBNull(5))
                                chamado.DataAbertura = DateTime.Parse(dataReader.GetString(5));

                        }
                        dataReader.Close();
                    }
                    dbConnection.Close();
                }

            }

            return chamado;
        }

        public bool GravarChamado(int ID, string Assunto, string Solicitante, int IdDepartamento, DateTime DataAbertura)
        {
            int regsAfetados = -1;

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    if (ID == 0)
                    {
                        dbCommand.CommandText = 
                            "INSERT INTO chamados (Assunto,Solicitante,IdDepartamento,DataAbertura)" +
                            "VALUES (@Assunto,@Solicitante,@IdDepartamento,@DataAbertura)";
                    }
                    else
                    {
                        dbCommand.CommandText = 
                            "UPDATE chamados " + 
                            "SET Assunto=@Assunto, " + 
                            "    Solicitante=@Solicitante, " +
                            "    IdDepartamento=@IdDepartamento, " + 
                            "    DataAbertura=@DataAbertura " + 
                            "WHERE ID=@ID ";
                    }

                    dbCommand.Parameters.AddWithValue("@Assunto", Assunto);
                    dbCommand.Parameters.AddWithValue("@Solicitante", Solicitante);
                    dbCommand.Parameters.AddWithValue("@IdDepartamento", IdDepartamento);
                    dbCommand.Parameters.AddWithValue("@DataAbertura", DataAbertura.ToString(ANSI_DATE_FORMAT));
                    dbCommand.Parameters.AddWithValue("@ID", ID);

                    dbConnection.Open();
                    regsAfetados = dbCommand.ExecuteNonQuery();
                    dbConnection.Close();
                }

            }

            return (regsAfetados > 0);

        }

        public bool ExcluirChamado(int idChamado)
        {
            int regsAfetados = -1;

            using (SQLiteConnection dbConnection = new SQLiteConnection(CONNECTION_STRING))
            {
                using (SQLiteCommand dbCommand = dbConnection.CreateCommand())
                {
                    dbCommand.CommandText = $"DELETE FROM chamados WHERE ID = {idChamado}";

                    dbConnection.Open();
                    regsAfetados = dbCommand.ExecuteNonQuery();
                    dbConnection.Close();

                }

            }

            return (regsAfetados > 0);
        }
    }
}