
﻿namespace WebApplicationsgls.Application.DTOs
{
    public class CriarProdutoDTO
    {
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public Guid CategoriaId { get; set; }
    }

}
