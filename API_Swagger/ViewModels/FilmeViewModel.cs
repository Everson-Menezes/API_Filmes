using API_Swagger.Enums;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace API_Swagger.ViewModels
{
    public class FilmeViewModel
    {
		// Atributos
		[Required]
		public Genero Genero { get; set; }
		[Required (ErrorMessage = "Por favor insira um título")]
		[StringLength(100, MinimumLength = 3, ErrorMessage = "O Título deve conter no minimo 3 caracteres e no máximo 100.")]
		public string Titulo { get; set; }
		[Required(ErrorMessage = "Por favor insira uma sinopse")]
		[StringLength(1000, MinimumLength = 3, ErrorMessage = "A sinopse deve conter no minimo 3 caracteres e no máximo 1000.")]
		public string Sinopse { get; set; }
		[Required(ErrorMessage = "Por favor insira uma data de lançamento")]
		public DateTime Data_Lancamento { get; set; }
		[Required(ErrorMessage = "Por favor insira uma produtora")]
		public string Produtora { get; set; }
		[Required(ErrorMessage = "Por favor insira um tempo de duração")]
		public int Duracao { get; set; }
		[Required(ErrorMessage = "Por favor insira um status")]
		public Status Status { get; set; }

	}
}