using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Infrastructuer.Migrations
{
    /// <inheritdoc />
    public partial class CategorySeedDataConfig : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Id", "Description", "Name" },
                values: new object[,]
                {
                    { new Guid("7c20c266-0969-431b-b0a3-bb1a65d4baa5"), "Problems related to internet connectivity, network devices, Wi-Fi issues, and communication systems.", "Networking" },
                    { new Guid("af51617f-64c4-409f-8867-749ca8a5ef83"), "Problems related to water systems including leaks, pipe blockages, drainage issues, and maintenance of plumbing fixtures.", "Plumbing" },
                    { new Guid("d8ed6dd0-83c5-4d78-93e9-58a138b8ccd8"), "Issues related to heating, ventilation, and air conditioning systems including cooling/heating failures and maintenance.", "HVAC" },
                    { new Guid("e8097122-bd95-4b96-bbe7-292241e044b8"), "All issues related to electrical systems, including power outages, wiring faults, lighting problems, and electrical equipment failures.", "Electrical" },
                    { new Guid("f21254f2-7cc3-4cea-886e-bcded6cf9492"), "Issues related to computers, software, systems, and technical support such as system errors, software installation, and device troubleshooting.", "InformationTechnology" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("7c20c266-0969-431b-b0a3-bb1a65d4baa5"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("af51617f-64c4-409f-8867-749ca8a5ef83"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("d8ed6dd0-83c5-4d78-93e9-58a138b8ccd8"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("e8097122-bd95-4b96-bbe7-292241e044b8"));

            migrationBuilder.DeleteData(
                table: "Categories",
                keyColumn: "Id",
                keyValue: new Guid("f21254f2-7cc3-4cea-886e-bcded6cf9492"));
        }
    }
}
