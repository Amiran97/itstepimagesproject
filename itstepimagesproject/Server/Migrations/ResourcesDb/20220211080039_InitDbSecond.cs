using Microsoft.EntityFrameworkCore.Migrations;

namespace itstepimagesproject.Server.Migrations.ResourcesDb
{
    public partial class InitDbSecond : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Profiles",
                columns: table => new
                {
                    Id = table.Column<string>(nullable: false, defaultValueSql: "NEWID()"),
                    Name = table.Column<string>(maxLength: 100, nullable: false),
                    PhotoUrl = table.Column<string>(nullable: true),
                    AppUserId = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Profiles", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Profiles");
        }
    }
}
