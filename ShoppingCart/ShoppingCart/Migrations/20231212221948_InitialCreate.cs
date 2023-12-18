using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ShoppingCart.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterDatabase()
                .Annotation("Npgsql:PostgresExtension:uuid-ossp", ",,");

            migrationBuilder.CreateTable(
                name: "Cartdb",
                columns: table => new
                {
                    ProductId = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Price = table.Column<double>(type: "double precision", nullable: false),
                    Quantity = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cartdb", x => x.ProductId);
                });

            migrationBuilder.CreateTable(
                name: "CartPriceTotaldb",
                columns: table => new
                {
                    Id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    TotalPrice = table.Column<double>(type: "double precision", nullable: false),
                    TotalCartItems = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CartPriceTotaldb", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "CartPriceTotaldb",
                columns: new[] { "Id", "TotalCartItems", "TotalPrice" },
                values: new object[] { 1, 0, 0.0 });

            migrationBuilder.InsertData(
                table: "Cartdb",
                columns: new[] { "ProductId", "Name", "Price", "Quantity" },
                values: new object[] { 1, "Adidas Shoe", 200.0, 1L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cartdb");

            migrationBuilder.DropTable(
                name: "CartPriceTotaldb");
        }
    }
}
