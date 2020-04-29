using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Routine.Api.Migrations
{
    public partial class AddEmployeeData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("f6a41019-4697-4048-ba96-cea83f852e6a"), new Guid("f6a41019-4697-4048-ba96-ae94f58efec9"), new DateTime(1990, 4, 27, 18, 27, 42, 486, DateTimeKind.Local).AddTicks(9002), "MSF324", "Li", 2, "si" });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CompanyId", "DateOfBirth", "EmployeeNo", "FirstName", "Gender", "LastName" },
                values: new object[] { new Guid("1afca17b-4697-4048-af76-cea83f852e6a"), new Guid("f6a41019-4697-4048-ba96-ae94f58efec9"), new DateTime(2000, 4, 27, 18, 27, 42, 487, DateTimeKind.Local).AddTicks(6030), "MSF314", "Wang", 1, "wu" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("1afca17b-4697-4048-af76-cea83f852e6a"));

            migrationBuilder.DeleteData(
                table: "Employees",
                keyColumn: "Id",
                keyValue: new Guid("f6a41019-4697-4048-ba96-cea83f852e6a"));
        }
    }
}
