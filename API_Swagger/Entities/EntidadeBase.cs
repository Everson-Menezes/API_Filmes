using API_Swagger.Enums;
using System;

namespace API_Swagger.Entities
{
    public abstract class EntidadeBase
	{
		// Atributos
		public int Id { get; set; }
		public Genero Genero { get; set; }
		public string Titulo { get; set; }
		public string Sinopse { get; set; }
		public DateTime Data_Lancamento { get; set; }
		public Status Status { get; set; }
        public bool StatusRegistro { get; set; }
    }
}