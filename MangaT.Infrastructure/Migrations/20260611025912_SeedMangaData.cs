using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace MangaT.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedMangaData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Mangas",
                columns: new[] { "Id", "Author", "Category", "CreatedDate", "Description", "DetailUrl", "ImageUrl", "Point", "Title", "VolumeCount" },
                values: new object[,]
                {
                    { 1, "Eiichiro Oda", "Adventure", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A story about a group of pirates searching for the ultimate treasure.", "https://example.com/onepiece", "https://example.com/onepiece.jpg", 9.5, "One Piece", 100 },
                    { 2, "Masashi Kishimoto", "Action", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A young ninja's journey to become the strongest in his village.", "https://example.com/naruto", "https://example.com/naruto.jpg", 9.0, "Naruto", 72 },
                    { 3, "Tite Kubo", "Fantasy", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "A teenager gains the powers of a Soul Reaper and battles evil spirits.", "https://example.com/bleach", "https://example.com/bleach.jpg", 8.5, "Bleach", 74 },
                    { 4, "Hajime Isayama", "Horror", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "Humanity's fight for survival against giant humanoid creatures.", "https://example.com/attackontitan", "https://example.com/attackontitan.jpg", 9.8000000000000007, "Attack on Titan", 34 },
                    { 5, "Kohei Horikoshi", "Superhero", new DateTime(2026, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc), "In a world where superpowers are the norm, a boy without them strives to be a hero.", "https://example.com/myheroacademia", "https://example.com/myheroacademia.jpg", 9.1999999999999993, "My Hero Academia", 30 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Mangas",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Mangas",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Mangas",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Mangas",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Mangas",
                keyColumn: "Id",
                keyValue: 5);
        }
    }
}
