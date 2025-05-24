using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace APBD_11.Migrations
{
    /// <inheritdoc />
    public partial class AddSeedingData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Doctor",
                columns: new[] { "IdDoctor", "Email", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, "doctor1@ex.com", "Doctor 1", "1" },
                    { 2, "doctor2@ex.com", "Doctor 2", "2" },
                    { 3, "doctor3@ex.com", "Doctor 3", "3" },
                    { 4, "doctor4@ex.com", "Doctor 4", "4" },
                    { 5, "doctor5@ex.com", "Doctor 5", "5" }
                });

            migrationBuilder.InsertData(
                table: "Medicament",
                columns: new[] { "IdMedicament", "Description", "Name", "Type" },
                values: new object[,]
                {
                    { 1, "Very nice", "Medicament 1", "Type 2" },
                    { 2, "Awesome", "Medicament 2", "Type 1" },
                    { 3, "Perfect medicament", "Medicament 3", "Type 2" }
                });

            migrationBuilder.InsertData(
                table: "Patient",
                columns: new[] { "IdPatient", "Birthdate", "FirstName", "LastName" },
                values: new object[,]
                {
                    { 1, new DateTime(2000, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Patient 1", "1" },
                    { 2, new DateTime(2001, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Patient 2", "2" },
                    { 3, new DateTime(2002, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Patient 3", "3" },
                    { 4, new DateTime(2003, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Patient 4", "4" },
                    { 5, new DateTime(2004, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), "Patient 5", "5" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Doctor",
                keyColumn: "IdDoctor",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Doctor",
                keyColumn: "IdDoctor",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Doctor",
                keyColumn: "IdDoctor",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Doctor",
                keyColumn: "IdDoctor",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Doctor",
                keyColumn: "IdDoctor",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Medicament",
                keyColumn: "IdMedicament",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Medicament",
                keyColumn: "IdMedicament",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Medicament",
                keyColumn: "IdMedicament",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "IdPatient",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "IdPatient",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "IdPatient",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "IdPatient",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Patient",
                keyColumn: "IdPatient",
                keyValue: 5);
        }
    }
}
