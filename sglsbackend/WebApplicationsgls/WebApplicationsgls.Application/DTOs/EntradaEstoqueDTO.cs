namespace WebApplicationsgls.Application.DTOs
{
    public class EntradaEstoqueDTO
    {
        public Guid ProdutoId { get; set; }
        public int Quantidade { get; set; }
        public string? Observacao { get; set; }
    }
}
