using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class fixdefaultadminpassword : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Login",
                keyValue: "admin",
                column: "HashedPassword",
                value: "$2a$11$Kmc9xqbxY41lkG4Gr6l4fe7a.z6YX8k9T4GOIa4HMt5q5OR6J0Lf6");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Login",
                keyValue: "admin",
                column: "HashedPassword",
                value: "$2a$12$jKg1PDYGJudrCUJP7rr4FOXe.EP9s4CLJcMf23rPsSg45QKwvSFGi");
        }
    }
}
