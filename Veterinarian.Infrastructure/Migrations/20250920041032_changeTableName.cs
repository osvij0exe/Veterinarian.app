using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veterinarian.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class changeTableName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invoices_User_UserId",
                schema: "Veterinarian",
                table: "invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalConsultationUser_User_UserId",
                schema: "Veterinarian",
                table: "MedicalConsultationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Veterinarians_User_UserId",
                schema: "Veterinarian",
                table: "Veterinarians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_User",
                schema: "Veterinarian",
                table: "User");

            migrationBuilder.RenameTable(
                name: "User",
                schema: "Veterinarian",
                newName: "Users",
                newSchema: "Veterinarian");

            migrationBuilder.RenameIndex(
                name: "IX_User_IdentityId",
                schema: "Veterinarian",
                table: "Users",
                newName: "IX_Users_IdentityId");

            migrationBuilder.RenameIndex(
                name: "IX_User_Email",
                schema: "Veterinarian",
                table: "Users",
                newName: "IX_Users_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Users",
                schema: "Veterinarian",
                table: "Users",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_invoices_Users_UserId",
                schema: "Veterinarian",
                table: "invoices",
                column: "UserId",
                principalSchema: "Veterinarian",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalConsultationUser_Users_UserId",
                schema: "Veterinarian",
                table: "MedicalConsultationUser",
                column: "UserId",
                principalSchema: "Veterinarian",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Veterinarians_Users_UserId",
                schema: "Veterinarian",
                table: "Veterinarians",
                column: "UserId",
                principalSchema: "Veterinarian",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_invoices_Users_UserId",
                schema: "Veterinarian",
                table: "invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_MedicalConsultationUser_Users_UserId",
                schema: "Veterinarian",
                table: "MedicalConsultationUser");

            migrationBuilder.DropForeignKey(
                name: "FK_Veterinarians_Users_UserId",
                schema: "Veterinarian",
                table: "Veterinarians");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Users",
                schema: "Veterinarian",
                table: "Users");

            migrationBuilder.RenameTable(
                name: "Users",
                schema: "Veterinarian",
                newName: "User",
                newSchema: "Veterinarian");

            migrationBuilder.RenameIndex(
                name: "IX_Users_IdentityId",
                schema: "Veterinarian",
                table: "User",
                newName: "IX_User_IdentityId");

            migrationBuilder.RenameIndex(
                name: "IX_Users_Email",
                schema: "Veterinarian",
                table: "User",
                newName: "IX_User_Email");

            migrationBuilder.AddPrimaryKey(
                name: "PK_User",
                schema: "Veterinarian",
                table: "User",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_invoices_User_UserId",
                schema: "Veterinarian",
                table: "invoices",
                column: "UserId",
                principalSchema: "Veterinarian",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MedicalConsultationUser_User_UserId",
                schema: "Veterinarian",
                table: "MedicalConsultationUser",
                column: "UserId",
                principalSchema: "Veterinarian",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Veterinarians_User_UserId",
                schema: "Veterinarian",
                table: "Veterinarians",
                column: "UserId",
                principalSchema: "Veterinarian",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
