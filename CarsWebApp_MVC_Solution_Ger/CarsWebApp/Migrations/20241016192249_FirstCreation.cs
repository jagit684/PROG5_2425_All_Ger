using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CarsWebApp.Migrations
{
    /// <inheritdoc />
    public partial class FirstCreation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CarColors",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Color = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarColors", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "CarOptions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Option = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarOptions", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Cars",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false),
                    Brand = table.Column<string>(type: "nvarchar(15)", maxLength: 15, nullable: false),
                    Type = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Price = table.Column<int>(type: "int", nullable: false),
                    Year = table.Column<int>(type: "int", nullable: false),
                    CarColorId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cars", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cars_CarColors_CarColorId",
                        column: x => x.CarColorId,
                        principalTable: "CarColors",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "CarHasOptions",
                columns: table => new
                {
                    CarId = table.Column<int>(type: "int", nullable: false),
                    CarOptionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CarHasOptions", x => new { x.CarId, x.CarOptionId });
                    table.ForeignKey(
                        name: "FK_CarHasOptions_CarOptions_CarOptionId",
                        column: x => x.CarOptionId,
                        principalTable: "CarOptions",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_CarHasOptions_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "CarColors",
                columns: new[] { "Id", "Color" },
                values: new object[,]
                {
                    { 1, "Red" },
                    { 2, "Blue" },
                    { 3, "Black" },
                    { 4, "Gold" },
                    { 5, "Silver" }
                });

            migrationBuilder.InsertData(
                table: "CarOptions",
                columns: new[] { "Id", "Option", "Price" },
                values: new object[,]
                {
                    { 1, "Leather Seats", 1500 },
                    { 2, "Sunroof", 1200 },
                    { 3, "Navigation System", 1000 },
                    { 4, "Heated Seats", 500 },
                    { 5, "Bluetooth", 300 },
                    { 6, "Backup Camera", 200 },
                    { 7, "Remote Start", 400 },
                    { 8, "Blind Spot Monitoring", 600 },
                    { 9, "Parking Sensors", 400 },
                    { 10, "Lane Departure Warning", 1500 },
                    { 11, "Adaptive Cruise Control", 1500 },
                    { 12, "Apple CarPlay", 300 },
                    { 13, "Android Auto", 300 },
                    { 14, "Wi-Fi Hotspot", 200 },
                    { 15, "Keyless Entry", 300 },
                    { 16, "Power Liftgate", 500 },
                    { 17, "Satellite Radio", 300 },
                    { 18, "Automatic Parking", 1000 },
                    { 19, "Surround-View Camera", 800 },
                    { 20, "Head-Up Display", 1000 }
                });

            migrationBuilder.InsertData(
                table: "Cars",
                columns: new[] { "Id", "Brand", "CarColorId", "Price", "Type", "Year" },
                values: new object[,]
                {
                    { 1, "Chevrolet", 5, 38000, "Camaro", 2022 },
                    { 2, "Hyundai", 3, 19000, "Elantra", 2022 },
                    { 3, "Volkswagen", 3, 21500, "Passat", 2022 },
                    { 4, "Chevrolet", 4, 21000, "Equinox", 2022 },
                    { 5, "Volkswagen", 2, 19500, "Jetta", 2022 },
                    { 6, "Chevrolet", 1, 20000, "Malibu", 2022 },
                    { 7, "Hyundai", 4, 24000, "Santa Fe", 2022 },
                    { 8, "Volkswagen", 5, 21600, "Tiguan", 2022 },
                    { 9, "Chevrolet", 3, 20000, "Trailblazer", 2022 }
                });

            migrationBuilder.CreateIndex(
                name: "IX_CarHasOptions_CarOptionId",
                table: "CarHasOptions",
                column: "CarOptionId");

            migrationBuilder.CreateIndex(
                name: "IX_Cars_CarColorId",
                table: "Cars",
                column: "CarColorId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CarHasOptions");

            migrationBuilder.DropTable(
                name: "CarOptions");

            migrationBuilder.DropTable(
                name: "Cars");

            migrationBuilder.DropTable(
                name: "CarColors");
        }
    }
}
