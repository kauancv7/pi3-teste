namespace WebApplicationsgls.Application.DTOs
{
    public class ProdutoResponseDTO
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Descricao { get; set; }
        public decimal Preco { get; set; }
        public int Estoque { get; set; }
        public bool Ativo { get; set; }

        public Guid CategoriaId { get; set; }
        public string CategoriaNome { get; set; }
        public Guid? ImagemId { get; set; } 
    }
}