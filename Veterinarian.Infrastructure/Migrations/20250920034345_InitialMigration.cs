using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Veterinarian.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Veterinarian");

            migrationBuilder.CreateTable(
                name: "owners",
                schema: "Veterinarian",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    GivenName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_owners", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "pets",
                schema: "Veterinarian",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Specie = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Breed = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    GenderStatus = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    BirhtDate = table.Column<DateOnly>(type: "date", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pets", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Specialities",
                schema: "Veterinarian",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Specialities", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Veterinarian",
                columns: table => new
                {
                    Id = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Email = table.Column<string>(type: "nvarchar(300)", maxLength: 300, nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    CreateAtUtc = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdateAtUtc = table.Column<DateTime>(type: "datetime2", nullable: true),
                    IdentityId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "petOwners",
                schema: "Veterinarian",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    OwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_petOwners", x => x.Id);
                    table.ForeignKey(
                        name: "FK_petOwners_owners_OwnerId",
                        column: x => x.OwnerId,
                        principalSchema: "Veterinarian",
                        principalTable: "owners",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_petOwners_pets_PetId",
                        column: x => x.PetId,
                        principalSchema: "Veterinarian",
                        principalTable: "pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Veterinarians",
                schema: "Veterinarian",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    GivenName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    FamilyName = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    ProfessionalId = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Contact = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    SpecialityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Veterinarians", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Veterinarians_Specialities_SpecialityId",
                        column: x => x.SpecialityId,
                        principalSchema: "Veterinarian",
                        principalTable: "Specialities",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Veterinarians_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Veterinarian",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "medicalConsultations",
                schema: "Veterinarian",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AppointmentDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AppointmentEnd = table.Column<DateTime>(type: "datetime2", nullable: false),
                    MedicalTreatMent = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VetId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_medicalConsultations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_medicalConsultations_Veterinarians_VetId",
                        column: x => x.VetId,
                        principalSchema: "Veterinarian",
                        principalTable: "Veterinarians",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_medicalConsultations_pets_PetId",
                        column: x => x.PetId,
                        principalSchema: "Veterinarian",
                        principalTable: "pets",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "invoices",
                schema: "Veterinarian",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false),
                    Amount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    PaymentMethod = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Paid = table.Column<bool>(type: "bit", nullable: false),
                    MedicalConsultationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_invoices", x => x.Id);
                    table.ForeignKey(
                        name: "FK_invoices_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Veterinarian",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_invoices_medicalConsultations_MedicalConsultationId",
                        column: x => x.MedicalConsultationId,
                        principalSchema: "Veterinarian",
                        principalTable: "medicalConsultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MedicalConsultationUser",
                schema: "Veterinarian",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    MedcialConsultationId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MedicalConsultationUser", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MedicalConsultationUser_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Veterinarian",
                        principalTable: "User",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MedicalConsultationUser_medicalConsultations_MedcialConsultationId",
                        column: x => x.MedcialConsultationId,
                        principalSchema: "Veterinarian",
                        principalTable: "medicalConsultations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_invoices_MedicalConsultationId",
                schema: "Veterinarian",
                table: "invoices",
                column: "MedicalConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_invoices_UserId",
                schema: "Veterinarian",
                table: "invoices",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_medicalConsultations_PetId",
                schema: "Veterinarian",
                table: "medicalConsultations",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_medicalConsultations_VetId",
                schema: "Veterinarian",
                table: "medicalConsultations",
                column: "VetId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConsultationUser_MedcialConsultationId",
                schema: "Veterinarian",
                table: "MedicalConsultationUser",
                column: "MedcialConsultationId");

            migrationBuilder.CreateIndex(
                name: "IX_MedicalConsultationUser_UserId",
                schema: "Veterinarian",
                table: "MedicalConsultationUser",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_petOwners_OwnerId",
                schema: "Veterinarian",
                table: "petOwners",
                column: "OwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_petOwners_PetId",
                schema: "Veterinarian",
                table: "petOwners",
                column: "PetId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "Veterinarian",
                table: "User",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_IdentityId",
                schema: "Veterinarian",
                table: "User",
                column: "IdentityId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Veterinarians_SpecialityId",
                schema: "Veterinarian",
                table: "Veterinarians",
                column: "SpecialityId");

            migrationBuilder.CreateIndex(
                name: "IX_Veterinarians_UserId",
                schema: "Veterinarian",
                table: "Veterinarians",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "invoices",
                schema: "Veterinarian");

            migrationBuilder.DropTable(
                name: "MedicalConsultationUser",
                schema: "Veterinarian");

            migrationBuilder.DropTable(
                name: "petOwners",
                schema: "Veterinarian");

            migrationBuilder.DropTable(
                name: "medicalConsultations",
                schema: "Veterinarian");

            migrationBuilder.DropTable(
                name: "owners",
                schema: "Veterinarian");

            migrationBuilder.DropTable(
                name: "Veterinarians",
                schema: "Veterinarian");

            migrationBuilder.DropTable(
                name: "pets",
                schema: "Veterinarian");

            migrationBuilder.DropTable(
                name: "Specialities",
                schema: "Veterinarian");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Veterinarian");
        }
    }
}
