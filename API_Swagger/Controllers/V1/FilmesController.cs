using API_Swagger.Entities;
using API_Swagger.Interfaces;
using API_Swagger.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace API_Swagger.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class FilmesController : ControllerBase
    {
        private readonly IFilmeService _filmeService;

        public FilmesController(IFilmeService filmeService)
        {
            _filmeService = filmeService;
        }

        [HttpGet("BuscarTodos")]
        public async Task<ActionResult<List<object>>> BuscarTodos([FromQuery, Range(1, int.MaxValue)] int pag = 1, [FromQuery, Range(1, 10)] int qtdPag = 2)
        {
            try
            {
                var result = await _filmeService.BuscarTodos(pag, qtdPag);

                if (result.Count() == 0)
                    return NoContent();

                return Ok(result);
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
        [HttpGet("BuscarPorId{idFilme:int}")]
        public async Task<ActionResult<object>> Buscar([FromRoute] int idFilme)
        {
            try
            {
                var result = await _filmeService.Buscar(idFilme);

                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals(null))
                    throw new Exception("Ocorreu um erro inesperado, tente novamente mais tarde!");

                return NotFound(ex.Message);
            }
        }
        [HttpPost("Cadastrar")]
        public async Task<ActionResult<object>> Cadastrar([FromBody] FilmeViewModel objeto)
        {
            try
            {
                var result = await _filmeService.Cadastrar(objeto);
                return Created("api/v1/filmes/cadastrar", result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals(null))
                    throw new Exception("Ocorreu um erro inesperado, tente novamente mais tarde!");

                if (ex.Message.Contains("preenchidos"))
                    return BadRequest(ex.Message);

                if (ex.Message.Contains("cadastrado"))
                    return UnprocessableEntity(ex.Message);

                throw new Exception(ex.Message);
            }
        }
        [HttpPut("Atualizar{idFilme:int}")]
        public async Task<ActionResult> Atualizar([FromRoute] int idFilme, [FromBody] FilmeViewModel objeto)
        {
            try
            {
                var result = await _filmeService.Atualizar(idFilme, objeto);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals(null))
                    throw new Exception("Ocorreu um erro inesperado, tente novamente mais tarde!");

                if (ex.Message.Contains("preenchidos"))
                    return BadRequest(ex.Message);

                if (ex.Message.Contains("nulo"))
                    return BadRequest(ex.Message);

                return NotFound(ex.Message);
            }
        }
        [HttpPatch("AtualizarTitulo{idFilme:int}/{titulo}")]
        public async Task<ActionResult> Atualizar([FromRoute] int idFilme, [FromRoute] string titulo)
        {
            try
            {
                var result = await _filmeService.AtualizarTitulo(idFilme, titulo);
                return Ok(result);
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals(null))
                    throw new Exception("Ocorreu um erro inesperado, tente novamente mais tarde!");

                if (ex.Message.Contains("preenchidos"))
                    return BadRequest(ex.Message);

                if (ex.Message.Contains("nulo"))
                    return BadRequest(ex.Message);

                return NotFound(ex.Message);
            }
        }
        [HttpDelete("Excluir")]
        public async Task<ActionResult> Excluir(int id)
        {
            try
            {
                await _filmeService.Excluir(id);
                return Ok();
            }
            catch (Exception ex)
            {
                if (ex.Message.Equals(null))
                    throw new Exception("Ocorreu um erro inesperado, tente novamente mais tarde!");

                return NotFound(ex.Message);
            }
        }

    }
}
