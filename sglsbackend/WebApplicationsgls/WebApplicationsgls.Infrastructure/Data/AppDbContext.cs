using Microsoft.EntityFrameworkCore;
using WebApplicationsgls.Domain.Entities;

namespace WebApplicationsgls.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios { get; set; }
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<TipoCategoria> Categorias { get; set; }
    public DbSet<Produto> Produtos { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }
    public DbSet<MovimentacaoEstoque> MovimentacoesEstoque { get; set; }
    public DbSet<Image> Images { get; set; }
    public DbSet<Relatorio> Relatorios { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.HasKey(u => u.Id);

            entity.Property(u => u.Nome)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(u => u.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(u => u.Email)
                .IsUnique();

            entity.Property(u => u.Perfil)
                .IsRequired()
                .HasConversion<string>();
        });

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Email)
                .IsRequired()
                .HasMaxLength(150);

            entity.HasIndex(c => c.Email)
                .IsUnique();

            entity.Property(c => c.Telefone)
                .IsRequired()
                .HasMaxLength(20);
        });

        modelBuilder.Entity<TipoCategoria>(entity =>
        {
            entity.HasKey(c => c.Id);

            entity.Property(c => c.Nome)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(c => c.Ativa)
                .HasDefaultValue(true);
        });

        modelBuilder.Entity<Produto>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Nome)
                .IsRequired()
                .HasMaxLength(150);

            entity.Property(p => p.Preco)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(p => p.Categoria)
                .WithMany(c => c.Produtos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(p => p.Vendedor)
                .WithMany()
                .HasForeignKey(p => p.VendedorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(p => p.Id);

            entity.Property(p => p.Status)
                .IsRequired()
                .HasMaxLength(30);

            entity.Property(p => p.ValorTotal)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(p => p.Usuario)
                .WithMany()
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);

            entity.HasOne(p => p.Cliente)
                .WithMany()
                .HasForeignKey(p => p.ClienteId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });

        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.PrecoUnitario)
                .HasColumnType("decimal(18,2)");

            entity.HasOne(i => i.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.PedidoId);

            entity.HasOne(i => i.Produto)
                .WithMany()
                .HasForeignKey(i => i.ProdutoId);
        });


        modelBuilder.Entity<Image>(entity =>
        {
            entity.HasKey(i => i.Id);

            entity.Property(i => i.FileName)
                .IsRequired()
                .HasMaxLength(255);

            entity.Property(i => i.ContentType)
                .IsRequired()
                .HasMaxLength(100);

            entity.Property(i => i.FilePath)
                .IsRequired()
                .HasMaxLength(500);

            entity.Property(i => i.CreatedAt)
                .IsRequired();

            entity.Property(i => i.tipoImagemEnum)
                .IsRequired()
                .HasConversion<int>();

            entity.Property(i => i.OwnerId)
                .IsRequired();

            entity.HasIndex(i => new { i.tipoImagemEnum, i.OwnerId });
        });

        modelBuilder.Entity<MovimentacaoEstoque>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Tipo)
                .IsRequired();

            entity.Property(m => m.Quantidade)
                .IsRequired();

            entity.Property(m => m.Data)
                .IsRequired();

            entity.Property(m => m.Observacao)
                .HasMaxLength(255);

            entity.HasOne(m => m.Produto)
                .WithMany()
                .HasForeignKey(m => m.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Usuario)
                .WithMany()
                .HasForeignKey(m => m.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict)
                .IsRequired(false);
        });

        modelBuilder.Entity<Relatorio>(entity =>
        {
            entity.HasKey(m => m.Id);

            entity.Property(m => m.Id)
                .IsRequired();

            entity.Property(m => m.Descricao)
                .IsRequired();

            entity.Property(m => m.DataCriacao)
                .IsRequired();

            entity.HasOne(m => m.Produto)
                .WithMany()
                .HasForeignKey(m => m.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);

            entity.HasOne(m => m.Usuario)
                .WithMany()
                .HasForeignKey(m => m.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        });


    }
}
