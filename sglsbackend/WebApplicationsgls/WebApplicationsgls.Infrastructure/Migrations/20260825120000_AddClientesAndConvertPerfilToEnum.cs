using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplicationsgls.Infrastructure.Migrations;

public partial class AddClientesAndConvertPerfilToEnum : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.Sql("""
            ALTER TABLE \"Usuarios\"
            ALTER COLUMN \"Perfil\" TYPE integer
            USING CASE \"Perfil\"
                WHEN 'Admin' THEN 1
                WHEN 'Vendedor' THEN 2
                WHEN 'Marketing' THEN 3
                WHEN '1' THEN 1
                WHEN '2' THEN 2
                WHEN '3' THEN 3
                ELSE 1
            END;
            """);

        migrationBuilder.CreateTable(
            name: "Clientes",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "uuid", nullable: false),
                Nome = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                Email = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                SenhaHash = table.Column<string>(type: "text", nullable: false),
                Telefone = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Clientes", x => x.Id));

        migrationBuilder.CreateIndex(
            name: "IX_Clientes_Email",
            table: "Clientes",
            column: "Email",
            unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.DropTable(name: "Clientes");

        migrationBuilder.Sql("""
            ALTER TABLE \"Usuarios\"
            ALTER COLUMN \"Perfil\" TYPE character varying(20)
            USING CASE \"Perfil\"
                WHEN 1 THEN 'Admin'
                WHEN 2 THEN 'Vendedor'
                WHEN 3 THEN 'Marketing'
                ELSE 'Admin'
            END;
            """);
    }
}
