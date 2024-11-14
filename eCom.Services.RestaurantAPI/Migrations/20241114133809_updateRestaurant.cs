using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace eCom.Services.RestaurantAPI.Migrations
{
    /// <inheritdoc />
    public partial class updateRestaurant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "RestaurantRating",
                table: "Restaurants",
                type: "float",
                nullable: false,
                oldClrType: typeof(float),
                oldType: "real");

            migrationBuilder.InsertData(
                table: "Restaurants",
                columns: new[] { "RestaurantId", "RestaurantDescription", "RestaurantLocation", "RestaurantName", "RestaurantRating" },
                values: new object[] { 1, " Quisque vel lacus ac magna, vehicula sagittis ut non lacus.<br/> Vestibulum arcu turpis, maximus malesuada neque. Phasellus commodo cursus pretium.", "Da Nang", "Ramen Ichido", 4.2999999999999998 });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Restaurants",
                keyColumn: "RestaurantId",
                keyValue: 1);

            migrationBuilder.AlterColumn<float>(
                name: "RestaurantRating",
                table: "Restaurants",
                type: "real",
                nullable: false,
                oldClrType: typeof(double),
                oldType: "float");
        }
    }
}
