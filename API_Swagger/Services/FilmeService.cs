using API_Swagger.Entities;
using API_Swagger.Enums;
using API_Swagger.Interfaces;
using API_Swagger.Repositories;
using API_Swagger.ViewModels;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace API_Swagger.Services
{
    public class FilmeService : IFilmeService
    {
        private readonly IFilmeRepository _filmeRepository;

        public FilmeService(IFilmeRepository filmeRepository)
        {
            _filmeRepository = filmeRepository;
        }

        public async Task<FilmeViewModel> Atualizar(int id, FilmeViewModel objeto)
        {
            if (!ViewModelValidada(objeto))
                throw new Exception("Todos os campos devem ser preenchidos");

            if (id.Equals(null))
                throw new Exception("id não pode ser nulo");

            try
            {
                var result = await _filmeRepository.GetById(id);

                result.Id = id;
                result.Genero = objeto.Genero;
                result.Titulo = objeto.Titulo;
                result.Sinopse = objeto.Sinopse;
                result.Data_Lancamento = objeto.Data_Lancamento;
                result.Produtora = objeto.Produtora;
                result.Duracao = objeto.Duracao;
                result.Status = objeto.Status;

                await _filmeRepository.Update(result.Id, result);

                return objeto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

        }

        public async Task<FilmeViewModel> AtualizarTitulo(int id, string titulo)
        {
            if (id.Equals(null) || id == 0)
                throw new Exception("O parâmetro id não pode ser nulo ou conter o valor zero");

            if (titulo.Equals("") || titulo.Equals(null))
                throw new Exception("O parâmetro título não pode estar vazio ou nulo");

            try
            {
                var result = await _filmeRepository.GetById(id);

                result.Id = id;
                result.Titulo = titulo;
                result = await _filmeRepository.Update(result.Id, result.Titulo);

                FilmeViewModel viewModel = new FilmeViewModel()
                {
                    Titulo = result.Titulo,
                    Genero = result.Genero,
                    Sinopse = result.Sinopse,
                    Duracao = result.Duracao,
                    Produtora = result.Produtora,
                    Data_Lancamento = result.Data_Lancamento,
                    Status = result.Status,
                };
                return viewModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<FilmeViewModel> Buscar(int id)
        {
            if (id.Equals(null) || id == 0)
                throw new Exception("O parâmetro id não pode ser nulo ou conter o valor zero");

            try
            {
                var result = await _filmeRepository.GetById(id);

                FilmeViewModel viewModel = new FilmeViewModel()
                {
                    Titulo = result.Titulo,
                    Genero = result.Genero,
                    Sinopse = result.Sinopse,
                    Duracao = result.Duracao,
                    Produtora = result.Produtora,
                    Data_Lancamento = result.Data_Lancamento,
                    Status = result.Status,
                };
                return viewModel;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public async Task<List<FilmeViewModel>> BuscarTodos(int pagina, int quantidadePorPagina)
        {
            var result = await _filmeRepository.GetAll(pagina, quantidadePorPagina);

            var viewModels = new List<FilmeViewModel>();

            foreach (var item in result)
            {
                FilmeViewModel filme = new FilmeViewModel()
                {
                    Titulo = item.Titulo,
                    Genero = item.Genero,
                    Sinopse = item.Sinopse,
                    Duracao = item.Duracao,
                    Produtora = item.Produtora,
                    Data_Lancamento = item.Data_Lancamento,
                    Status = item.Status
                };
                viewModels.Add(filme);
            }

            return viewModels;
        }

        public async Task<FilmeViewModel> Cadastrar(FilmeViewModel objeto)
        {
            if (!ViewModelValidada(objeto))
                throw new Exception("Todos os campos devem ser preenchidos");

            Filme filme = new Filme()
            {
                Titulo = objeto.Titulo,
                Genero = objeto.Genero,
                Sinopse = objeto.Sinopse,
                Duracao = objeto.Duracao,
                Produtora = objeto.Produtora,
                Data_Lancamento = objeto.Data_Lancamento,
                Status = objeto.Status,
                StatusRegistro = true
            };
            try
            {
                await _filmeRepository.Create(filme);
                return objeto;
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void Dispose()
        {
            _filmeRepository?.Dispose();
        }

        public Task Excluir(int id)
        {
            try
            {
                return _filmeRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public bool ViewModelValidada(FilmeViewModel objeto)
        {
            if (
               objeto.Titulo == null ||
               objeto.Genero.Equals(null) || objeto.Genero == 0 ||
               objeto.Sinopse == null ||
               objeto.Duracao.Equals(null) || objeto.Duracao == 0 ||
               objeto.Produtora == null ||
               objeto.Data_Lancamento.Equals(null) ||
               objeto.Status.Equals(null) || objeto.Status == 0
              )
                return false;

            return true;
        }
    }
}
