using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace GymPortal.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedGymClasses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "GymClasses",
                columns: new[] { "Id", "Capacity", "Category", "Instructor", "Name", "StartTime" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), 20, "Rörlighet", "Emma", "Yoga", new DateTime(2026, 5, 5, 18, 0, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("22222222-2222-2222-2222-222222222222"), 15, "Kondition", "Ali", "Spinning", new DateTime(2026, 5, 6, 17, 30, 0, 0, DateTimeKind.Unspecified) },
                    { new Guid("33333333-3333-3333-3333-333333333333"), 12, "Styrka/Kondition", "Sara", "HIIT", new DateTime(2026, 5, 7, 19, 0, 0, 0, DateTimeKind.Unspecified) }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "GymClasses",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));
        }
    }
}
