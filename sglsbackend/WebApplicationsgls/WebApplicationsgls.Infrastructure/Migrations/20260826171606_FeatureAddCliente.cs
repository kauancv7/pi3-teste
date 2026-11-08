using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApplicationsgls.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class FeatureAddCliente : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ImageType",
                table: "Images",
                newName: "tipoImagemEnum");

            migrationBuilder.RenameIndex(
                name: "IX_Images_ImageType_OwnerId",
                table: "Images",
                newName: "IX_Images_tipoImagemEnum_OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "tipoImagemEnum",
                table: "Images",
                newName: "ImageType");

            migrationBuilder.RenameIndex(
                name: "IX_Images_tipoImagemEnum_OwnerId",
                table: "Images",
                newName: "IX_Images_ImageType_OwnerId");
        }
    }
}
