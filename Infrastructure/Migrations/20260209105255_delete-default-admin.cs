using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class deletedefaultadmin : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Login",
                keyValue: "admin");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Login", "HashedPassword", "RoleId" },
                values: new object[] { "admin", "$2a$11$Kmc9xqbxY41lkG4Gr6l4fe7a.z6YX8k9T4GOIa4HMt5q5OR6J0Lf6", 2 });
        }
    }
}
