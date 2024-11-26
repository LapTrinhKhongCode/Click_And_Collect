using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCom.Services.AuthAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateRestaurantOwnerIdRow : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RestaurantOwnerId",
                table: "AspNetUsers",
                type: "int",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RestaurantOwnerId",
                table: "AspNetUsers");
        }
    }
}
