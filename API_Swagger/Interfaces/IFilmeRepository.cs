using API_Swagger.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Swagger.Interfaces
{
    public interface IFilmeRepository : IDisposable
    {
        Task<List<Filme>> GetAll(int pagina, int quantidadePorPagina);
        Task<Filme> GetById(int id);
        Task<Filme> Create(Filme objeto);
        Task<Filme> Update(int id, Filme objeto);
        Task<Filme> Update(int id, string conteudo);
        Task Delete(int id);
        int NextId();
        bool DataExist(int id);
        bool DataExist(string titulo, string produtora);
    }
}