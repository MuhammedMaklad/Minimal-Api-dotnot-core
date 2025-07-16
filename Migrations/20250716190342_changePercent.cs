using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MinimalApi.Migrations
{
    /// <inheritdoc />
    public partial class changePercent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Percent",
                table: "Coupons",
                type: "decimal(5,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(2,2)");

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2025, 7, 16, 19, 3, 40, 935, DateTimeKind.Utc).AddTicks(7763), new DateTime(2025, 7, 16, 19, 3, 40, 935, DateTimeKind.Utc).AddTicks(7771) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2025, 7, 16, 19, 3, 40, 935, DateTimeKind.Utc).AddTicks(7777), new DateTime(2025, 7, 16, 19, 3, 40, 935, DateTimeKind.Utc).AddTicks(7778) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "Percent",
                table: "Coupons",
                type: "decimal(2,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(5,2)");

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2025, 7, 13, 7, 44, 55, 598, DateTimeKind.Utc).AddTicks(4134), new DateTime(2025, 7, 13, 7, 44, 55, 598, DateTimeKind.Utc).AddTicks(4140) });

            migrationBuilder.UpdateData(
                table: "Coupons",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Created", "LastUpdated" },
                values: new object[] { new DateTime(2025, 7, 13, 7, 44, 55, 598, DateTimeKind.Utc).AddTicks(4143), new DateTime(2025, 7, 13, 7, 44, 55, 598, DateTimeKind.Utc).AddTicks(4144) });
        }
    }
}
