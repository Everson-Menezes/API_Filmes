using API_Swagger.Entities;
using API_Swagger.Enums;
using API_Swagger.Interfaces;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace API_Swagger.Repositories
{
    public class FilmeSqlServerRepository : IFilmeRepository
    {
        private readonly SqlConnection _sqlConnection;

        public FilmeSqlServerRepository(IConfiguration configuration)
        {
            _sqlConnection = new SqlConnection(configuration.GetConnectionString("SQLServerDefault"));
        }

        public async Task<Filme> Create(Filme objeto)
        {
            await _sqlConnection.OpenAsync();
            try
            {
                if (DataExist(objeto.Titulo, objeto.Produtora))
                    throw new Exception("Filme já cadastrado!");

                objeto.Id = NextId();

                string query = "INSERT INTO TB_FILMES (ID, GENERO, TITULO, SINOPSE, PRODUTORA, DATA_LANCAMENTO, DURACAO, STATUS_FILME, STATUS_REGISTRO) " +
                                 "VALUES (@id, @genero, @titulo, @sinopse, @produtora, @data, @duracao, @statusFilme, @statusRegistro)";

                SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);

                sqlCommand.Parameters.AddWithValue("@id", objeto.Id);
                sqlCommand.Parameters.AddWithValue("@genero", objeto.Genero);
                sqlCommand.Parameters.AddWithValue("@titulo", objeto.Titulo);
                sqlCommand.Parameters.AddWithValue("@sinopse", objeto.Sinopse);
                sqlCommand.Parameters.AddWithValue("@produtora", objeto.Produtora);
                sqlCommand.Parameters.AddWithValue("@data", objeto.Data_Lancamento);
                sqlCommand.Parameters.AddWithValue("@duracao", objeto.Duracao);
                sqlCommand.Parameters.AddWithValue("@statusFilme", objeto.Status);
                sqlCommand.Parameters.AddWithValue("@statusRegistro", objeto.StatusRegistro);
                sqlCommand.ExecuteNonQuery();

                return objeto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _sqlConnection.Dispose();
                _sqlConnection.Close();
            }
        }

        public bool DataExist(int id)
        {
            string query = $"SELECT * FROM TB_FILMES WHERE ID = '{id}'";
            SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
            SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();
            while (sqlDataReader.Read())
            {
                if(sqlDataReader.GetValue(9).ToString() == "False")
                return false;
            }

            return true;
        }

        public bool DataExist(string titulo, string produtora)
        {
            try
            {
                string query = "SELECT TITULO, PRODUTORA FROM TB_FILMES WHERE TITULO = @titulo AND PRODUTORA = @produtora";
                SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
                sqlCommand.Parameters.AddWithValue("@titulo", titulo);
                sqlCommand.Parameters.AddWithValue("@produtora", produtora);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    if (sqlDataReader["TITULO"].Equals(titulo) && sqlDataReader["PRODUTORA"].Equals(produtora))
                        return true;
                }

            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return false;
        }

        public Task Delete(int id)
        {
            _sqlConnection.Open();
            try
            {
                if (!DataExist(id))
                    throw new Exception("Filme não encontrado!");

                string query = $"UPDATE TB_FILMES SET STATUS_REGISTRO = 0 WHERE ID = '{id}'";
                SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
                sqlCommand.ExecuteNonQuery();

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _sqlConnection.Dispose();
                _sqlConnection.Close();
            }
        }

        public void Dispose()
        {
            _sqlConnection?.Close();
            _sqlConnection?.Dispose();
        }

        public async Task<List<Filme>> GetAll(int pagina, int quantidadePorPagina)
        {
            List<Filme> filmes = new List<Filme>();

            await _sqlConnection.OpenAsync();
            try
            {
                string query = $"SELECT * FROM TB_FILMES WHERE STATUS_REGISTRO = 1 ORDER BY ID OFFSET {((pagina - 1) * quantidadePorPagina)} ROWS FETCH NEXT {quantidadePorPagina} ROWS ONLY";

                SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

                while (sqlDataReader.Read())
                {
                    filmes.Add(
                        new Filme
                        {
                            Titulo = (string)sqlDataReader["TITULO"],
                            Genero = (Genero)sqlDataReader["GENERO"],
                            Sinopse = (string)sqlDataReader["SINOPSE"],
                            Duracao = (int)sqlDataReader["DURACAO"],
                            Produtora = (string)sqlDataReader["PRODUTORA"],
                            Data_Lancamento = (DateTime)sqlDataReader["DATA_LANCAMENTO"],
                            Status = (Status)sqlDataReader["STATUS_FILME"],
                            StatusRegistro = (bool)sqlDataReader["STATUS_REGISTRO"],
                        });
                }
                return filmes;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _sqlConnection.Dispose();
                _sqlConnection.Close();
            }
        }

        public async Task<Filme> GetById(int id)
        {
            Filme filme = null;
            _sqlConnection.Open();
            try
            {
                if (!DataExist(id))
                    throw new Exception("Filme não encontrado!");

                string query = $"SELECT * FROM TB_FILMES WHERE ID = '{id}' AND STATUS_REGISTRO = 1";
                SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
                SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();
                while (sqlDataReader.Read())
                {
                    filme = new Filme
                    {
                        Titulo = (string)sqlDataReader["TITULO"],
                        Genero = (Genero)sqlDataReader["GENERO"],
                        Sinopse = (string)sqlDataReader["SINOPSE"],
                        Duracao = (int)sqlDataReader["DURACAO"],
                        Produtora = (string)sqlDataReader["PRODUTORA"],
                        Data_Lancamento = (DateTime)sqlDataReader["DATA_LANCAMENTO"],
                        Status = (Status)sqlDataReader["STATUS_FILME"],
                        StatusRegistro = (bool)sqlDataReader["STATUS_REGISTRO"],
                    };
                }
                return filme;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _sqlConnection.Dispose();
                _sqlConnection.Close();
            }            
        }

        public int NextId()
        {
            int ultimoId = 0;
            try
            {
                string query = "SELECT TOP (1) [ID] FROM TB_FILMES ORDER BY ID DESC";
                SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);
                SqlDataReader sqlDataReader = sqlCommand.ExecuteReader();

                while (sqlDataReader.Read())
                {
                    ultimoId = (int)sqlDataReader["ID"];
                }
            }
            catch (Exception ex)
            {
                if (ex.Message.Contains("Invalid attempt to read when no data is present."))
                    return ultimoId + 1;

                throw new Exception(ex.Message);
            }
            return ultimoId + 1;
        }

        public async Task<Filme> Update(int id, Filme objeto)
        {
            _sqlConnection.Open();

            try
            {
                if (!DataExist(id))
                throw new Exception("Filme não encontrado!");

                string query = "UPDATE TB_FILMES SET " +
                                        "GENERO = genero, " +
                                        "TITULO = @titulo, " +
                                        "SINOPSE = @sinopse, " +
                                        "PRODUTORA = @produtora, " +
                                        "DATA_LANCAMENTO = @data, " +
                                        "DURACAO = @duracao, " +
                                        "STATUS_FILME = @statusFilme, " +
                                        "STATUS_REGISTRO =  @statusRegistro " +
                                        "WHERE ID = @id";

                SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);

                sqlCommand.Parameters.AddWithValue("@id", objeto.Id);
                sqlCommand.Parameters.AddWithValue("@genero", objeto.Genero);
                sqlCommand.Parameters.AddWithValue("@titulo", objeto.Titulo);
                sqlCommand.Parameters.AddWithValue("@sinopse", objeto.Sinopse);
                sqlCommand.Parameters.AddWithValue("@produtora", objeto.Produtora);
                sqlCommand.Parameters.AddWithValue("@data", objeto.Data_Lancamento);
                sqlCommand.Parameters.AddWithValue("@duracao", objeto.Duracao);
                sqlCommand.Parameters.AddWithValue("@statusFilme", objeto.Status);
                sqlCommand.Parameters.AddWithValue("@statusRegistro", objeto.StatusRegistro);
                await sqlCommand.ExecuteNonQueryAsync();

                return objeto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _sqlConnection.Dispose();
                _sqlConnection.Close();
            }
        }

        public async Task<Filme> Update(int id, string titulo)
        {
            _sqlConnection.Open();

            try
            {
                if (!DataExist(id))
                    throw new Exception("Filme não encontrado!");

                string query = "UPDATE TB_FILMES SET " +
                                        "TITULO = @titulo, " +
                                        "WHERE ID = @id";

                SqlCommand sqlCommand = new SqlCommand(query, _sqlConnection);

                sqlCommand.Parameters.AddWithValue("@id", id);
                sqlCommand.Parameters.AddWithValue("@titulo", titulo);
                await sqlCommand.ExecuteNonQueryAsync();

                return await GetById(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            finally
            {
                _sqlConnection.Dispose();
                _sqlConnection.Close();
            }
        }
    }
}
