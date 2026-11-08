
﻿namespace WebApplicationsgls.Application.DTOs
{
    public class AtualizarProdutoDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public bool Ativo { get; set; }
        public Guid CategoriaId { get; set; }
    }

}
