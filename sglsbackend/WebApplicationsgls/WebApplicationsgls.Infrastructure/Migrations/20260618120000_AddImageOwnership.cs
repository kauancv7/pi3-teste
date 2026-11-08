using System;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using WebApplicationsgls.Infrastructure.Data;

#nullable disable

namespace WebApplicationsgls.Infrastructure.Migrations
{
    [DbContext(typeof(AppDbContext))]
    [Migration("20260618120000_AddImageOwnership")]
    public partial class AddImageOwnership : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ImageType",
                table: "Images",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "OwnerId",
                table: "Images",
                type: "uuid",
                nullable: false,
                defaultValue: Guid.Empty);

            migrationBuilder.CreateIndex(
                name: "IX_Images_ImageType_OwnerId",
                table: "Images",
                columns: new[] { "ImageType", "OwnerId" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Images_ImageType_OwnerId",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Images");

            migrationBuilder.DropColumn(
                name: "OwnerId",
                table: "Images");
        }
    }
}