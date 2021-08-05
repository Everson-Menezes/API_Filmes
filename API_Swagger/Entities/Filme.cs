using API_Swagger.Enums;
using System;

namespace API_Swagger.Entities
{
    public class Filme : EntidadeBase
    {
        public string Produtora { get; set; }
        public int Duracao { get; set; }

    }
}