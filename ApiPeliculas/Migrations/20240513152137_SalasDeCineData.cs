﻿using Microsoft.EntityFrameworkCore.Migrations;
using NetTopologySuite.Geometries;

#nullable disable

namespace ApiPeliculas.Migrations
{
    public partial class SalasDeCineData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "SalaDeCines",
                columns: new[] { "Id", "Nombre", "Ubicacion" },
                values: new object[] { 4, "Sambil", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-69.9118804 18.4826214)") });

            migrationBuilder.InsertData(
                table: "SalaDeCines",
                columns: new[] { "Id", "Nombre", "Ubicacion" },
                values: new object[] { 5, "Megacentro", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-69.856427 18.506934)") });

            migrationBuilder.InsertData(
                table: "SalaDeCines",
                columns: new[] { "Id", "Nombre", "Ubicacion" },
                values: new object[] { 6, "Village East Cinema", (NetTopologySuite.Geometries.Point)new NetTopologySuite.IO.WKTReader().Read("SRID=4326;POINT (-73.986227 40.730898)") });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "SalaDeCines",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "SalaDeCines",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "SalaDeCines",
                keyColumn: "Id",
                keyValue: 6);
        }
    }
}
