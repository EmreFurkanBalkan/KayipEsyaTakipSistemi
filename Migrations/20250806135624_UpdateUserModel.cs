using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LostAndFoundApp.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LostItems_Vehicles_VehicleId",
                table: "LostItems");

            migrationBuilder.DropTable(
                name: "Kullanicilar");

            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Vehicles");

            migrationBuilder.DropIndex(
                name: "IX_LostItems_VehicleId",
                table: "LostItems");

            migrationBuilder.DropColumn(
                name: "VehicleId",
                table: "LostItems");

            migrationBuilder.AddColumn<string>(
                name: "AdSoyad",
                table: "Users",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Rol",
                table: "Users",
                type: "TEXT",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Sifre",
                table: "Users",
                type: "TEXT",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "AdSoyad", "Rol", "Sifre" },
                values: new object[] { "admin", "Admin", "admin123" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "AdSoyad", "Rol", "Sifre" },
                values: new object[] { "Ahmet Yılmaz", "Kullanıcı", "123456" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "AdSoyad", "Rol", "Sifre" },
                values: new object[] { "Mehmet Demir", "Kullanıcı", "password" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 4,
                columns: new[] { "AdSoyad", "Rol", "Sifre" },
                values: new object[] { "Ayşe Kaya", "Moderatör", "test123" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: 5,
                columns: new[] { "AdSoyad", "Rol", "Sifre" },
                values: new object[] { "Fatma Özkan", "Kullanıcı", "demo123" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Rol",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "Sifre",
                table: "Users");

            migrationBuilder.AddColumn<int>(
                name: "VehicleId",
                table: "LostItems",
                type: "INTEGER",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Kullanicilar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    AdSoyad = table.Column<string>(type: "TEXT", nullable: false),
                    Rol = table.Column<string>(type: "TEXT", nullable: false),
                    Sifre = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Kullanicilar", x => x.Id);
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

            migrationBuilder.CreateTable(
                name: "Vehicles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    LineCode = table.Column<string>(type: "TEXT", nullable: false),
                    PlateNumber = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vehicles", x => x.Id);
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

            migrationBuilder.CreateIndex(
                name: "IX_LostItems_VehicleId",
                table: "LostItems",
                column: "VehicleId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LineCodeId",
                table: "Locations",
                column: "LineCodeId");

            migrationBuilder.CreateIndex(
                name: "IX_Locations_LostItemId",
                table: "Locations",
                column: "LostItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_LostItems_Vehicles_VehicleId",
                table: "LostItems",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id");
        }
    }
}
