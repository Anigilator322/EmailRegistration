using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmailRegistration.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModel3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VerificationCode",
                table: "AspNetUsers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "VerificationCode",
                table: "AspNetUsers",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
