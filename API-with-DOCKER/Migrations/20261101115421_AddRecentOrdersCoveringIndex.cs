using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace API_with_DOCKER.Migrations
{
    /// <inheritdoc />
    public partial class AddRecentOrdersCoveringIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Orders_CreatedAtUtc_Id",
                table: "Orders",
                columns: new[] { "CreatedAtUtc", "Id" },
                descending: new bool[0])
                .Annotation("SqlServer:Include", new[] { "CustomerId", "TotalCost" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Orders_CreatedAtUtc_Id",
                table: "Orders");
        }
    }
}
