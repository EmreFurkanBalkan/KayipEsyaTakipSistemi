using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LostAndFoundApp.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdSoyad = table.Column<string>(type: "TEXT", nullable: false),
                    Sifre = table.Column<string>(type: "TEXT", nullable: false),
                    Rol = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LineCodes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Line = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LineCodes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    UserName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    Password = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    PlateNumber = table.Column<string>(type: "TEXT", nullable: false),
                    LineCode = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LostItems",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    ItemName = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    FoundDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    LineCodeId = table.Column<int>(type: "INTEGER", nullable: true),
                    Location = table.Column<string>(type: "TEXT", nullable: false),
                    FoundBy = table.Column<string>(type: "TEXT", nullable: false),
                    VehicleId = table.Column<int>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LostItems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LostItems_LineCodes_LineCodeId",
                        column: x => x.LineCodeId,
                        principalTable: "LineCodes",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_LostItems_Vehicles_VehicleId",
                        column: x => x.VehicleId,
                        principalTable: "Vehicles",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LineCodeId = table.Column<int>(type: "INTEGER", nullable: false),
                    LostItemId = table.Column<int>(type: "INTEGER", nullable: true),
                    KapiID = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Locations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Locations_LineCodes_LineCodeId",
                        column: x => x.LineCodeId,
                        principalTable: "LineCodes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Locations_LostItems_LostItemId",
                        column: x => x.LostItemId,
                        principalTable: "LostItems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Kullanicilar",
                columns: new[] { "Id", "AdSoyad", "Rol", "Sifre" },
                values: new object[,]
                {
                    { 1, "admin", "Admin", "admin123" },
                    { 2, "Ahmet Yılmaz", "Kullanıcı", "123456" },
                    { 3, "Mehmet Demir", "Kullanıcı", "password" },
                    { 4, "Ayşe Kaya", "Moderatör", "test123" },
                    { 5, "Fatma Özkan", "Kullanıcı", "demo123" }
                });

            migrationBuilder.InsertData(
                table: "LineCodes",
                columns: new[] { "Id", "Line" },
                values: new object[,]
                {
                    { 1, "M1" },
                    { 2, "M2" },
                    { 3, "M3" },
                    { 4, "M4" },
                    { 5, "M5" },
                    { 6, "M6" },
                    { 7, "M7" },
                    { 8, "M8" },
                    { 9, "M9" },
                    { 10, "M10" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Password", "UserName" },
                values: new object[,]
                {
                    { 1, "admin123", "admin" },
                    { 2, "password1", "user1" },
                    { 3, "password2", "user2" },
                    { 4, "test123", "test" },
                    { 5, "demo123", "demo" }
                });

            migrationBuilder.InsertData(
                table: "Vehicles",
                columns: new[] { "Id", "LineCode", "PlateNumber" },
                values: new object[,]
                {
                    { 1, "M1", "34 ABC 123" },
                    { 2, "M2", "34 DEF 456" },
                    { 3, "M3", "34 GHI 789" },
                    { 4, "M4", "34 JKL 012" },
                    { 5, "M5", "34 MNO 345" },
                    { 6, "M6", "34 PQR 678" },
                    { 7, "M7", "34 STU 901" },
                    { 8, "M8", "34 VWX 234" },
                    { 9, "M9", "34 YZA 567" },
                    { 10, "M10", "34 BCD 890" }
                });

            migrationBuilder.InsertData(
                table: "Locations",
                columns: new[] { "Id", "KapiID", "LineCodeId", "LostItemId" },
                values: new object[,]
                {
                    { 1, "A1", 1, null },
                    { 2, "B2", 2, null },
                    { 3, "C3", 3, null },
                    { 4, "D4", 1, null },
                    { 5, "E5", 2, null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LineCodeId",
                table: "Locations",
                column: "LineCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LostItemId",
                table: "Locations",
                column: "LostItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LostItems_LineCodeId",
                table: "LostItems",
                column: "LineCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_LostItems_VehicleId",
                table: "LostItems",
                column: "VehicleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "LostItems");

            migrationBuilder.DropTable(
                name: "LineCodes");

            migrationBuilder.DropTable(
                name: "Vehicles");
        }
    }
}
