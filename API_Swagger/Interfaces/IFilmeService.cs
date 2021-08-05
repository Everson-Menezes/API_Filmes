using API_Swagger.Entities;
using API_Swagger.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API_Swagger.Interfaces
{
    public interface IFilmeService : IDisposable
    {
        Task<FilmeViewModel> Atualizar(int id, FilmeViewModel objeto);
        Task<FilmeViewModel> AtualizarTitulo(int id, string titulo);
        Task<FilmeViewModel> Buscar(int id);
        Task<List<FilmeViewModel>> BuscarTodos(int pagina, int quantidadePorPagina);
        Task<FilmeViewModel> Cadastrar(FilmeViewModel objeto);
        Task Excluir(int id);
        bool ViewModelValidada(FilmeViewModel objeto);
    }
}
