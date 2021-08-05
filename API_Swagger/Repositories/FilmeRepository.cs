using API_Swagger.Entities;
using API_Swagger.Enums;
using API_Swagger.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace API_Swagger.Repositories
{
    public class FilmeRepository : IFilmeRepository
    {
        //banco
        private static Dictionary<int, Filme> filmes = new Dictionary<int, Filme>()
        {
            {1, new Filme
                    {
                        Id = 1,
                        Genero = (Genero)1,
                        Titulo = "A origem",
                        Sinopse = "Em um mundo onde é possível entrar na mente humana, Cobb (Leonardo DiCaprio) está entre os melhores na arte de roubar segredos valiosos do inconsciente, durante o estado de sono. Além disto ele é um fugitivo, pois está impedido de retornar aos Estados Unidos devido à morte de Mal (Marion Cotillard). Desesperado para rever seus filhos, Cobb aceita a ousada missão proposta por Saito (Ken Watanabe), um empresário japonês: entrar na mente de Richard Fischer (Cillian Murphy), o herdeiro de um império econômico, e plantar a ideia de desmembrá-lo. Para realizar este feito ele conta com a ajuda do parceiro Arthur (Joseph Gordon-Levitt), a inexperiente arquiteta de sonhos Ariadne (Ellen Page) e Eames (Tom Hardy), que consegue se disfarçar de forma precisa no mundo dos sonhos.",
                        Data_Lancamento = new DateTime(2010, 7, 1),
                        Status = (Status)4,
                        Produtora = "Warner",
                        Duracao = 262,
                        StatusRegistro = true
                    }
            },
            {2, new Filme
                {
                    Id = 2,
                    Genero = (Genero)2,
                    Titulo = "Tropa de Elite",
                    Sinopse = "Policia de elite do rio de janeiro",
                    Data_Lancamento = new DateTime(2010, 7, 1),
                    Status = (Status)4,
                    Produtora = "Telecine",
                    Duracao = 262,
                    StatusRegistro = true
                }
            },
            {3, new Filme
                {
                    Id = 3,
                    Genero = (Genero)3,
                    Titulo = "Poderoso Chefão",
                    Sinopse = "Policia de elite do rio de janeiro",
                    Data_Lancamento = new DateTime(2010, 7, 1),
                    Status = (Status)4,
                    Produtora = "Telecine",
                    Duracao = 262,
                    StatusRegistro = true
                }
            }
        };

        public async Task<Filme> Create(Filme objeto)
        {
            if (DataExist(objeto.Titulo, objeto.Produtora))
                throw new Exception("Filme já cadastrado!");

            objeto.Id = NextId();
            filmes.Add(objeto.Id, objeto);
            return await Task.FromResult(filmes[objeto.Id]);
        }

        public bool DataExist(int id)
        {
            if (!filmes.ContainsKey(id) || filmes.Values.Where(f => f.Id == id).Select(f => f.StatusRegistro).FirstOrDefault() == false)
                return false;
            else
                return true;
        }
        public bool DataExist(string titulo, string produtora)
        {
            foreach (var item in filmes)
            {
                if (item.Value.Produtora.ToUpper().Equals(produtora.ToUpper()) && item.Value.Titulo.ToUpper().Equals(titulo.ToUpper()))
                    return true;
            }
            return false;
        }
        public Task Delete(int id)
        {
            if (!DataExist(id))
                throw new Exception("Filme não encontrado!");

            filmes[id].StatusRegistro = false;
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public Task<List<Filme>> GetAll(int pagina, int quantidadePorPagina)
        {
            return Task.FromResult(filmes.Values.Where(f => f.StatusRegistro == true).Skip((pagina - 1) * quantidadePorPagina).Take(quantidadePorPagina).ToList());
        }

        public async Task<Filme> GetById(int id)
        {
            if (!DataExist(id))
                throw new Exception("Filme não encontrado!");

            return await Task.FromResult(filmes[id]);
        }

        public int NextId()
        {
            return filmes.Count + 1;
        }

        public async Task<Filme> Update(int id, Filme objeto)
        {
            if (!DataExist(id))
                throw new Exception("Filme não cadastrado!");

            filmes[id].Id = id;
            filmes[id].Genero = objeto.Genero;
            filmes[id].Titulo = objeto.Titulo;
            filmes[id].Sinopse = objeto.Sinopse;
            filmes[id].Data_Lancamento = objeto.Data_Lancamento;
            filmes[id].Produtora = objeto.Produtora;
            filmes[id].Duracao = objeto.Duracao;
            filmes[id].Status = objeto.Status;

            return await Task.FromResult(filmes[id]);
        }

        public async Task<Filme> Update(int id, string conteudo)
        {
            if (!DataExist(id))
                throw new Exception("Filme não cadastrado!");

            filmes[id].Id = id;
            filmes[id].Titulo = conteudo;

            return await Task.FromResult(filmes[id]);
        }
    }
}
