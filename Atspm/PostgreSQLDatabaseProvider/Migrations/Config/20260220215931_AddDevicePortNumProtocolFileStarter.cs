using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utah.Udot.PostgreSQLDatabaseProvider.Migrations.Config
{
    /// <inheritdoc />
    public partial class AddDevicePortNumProtocolFileStarter : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "VersionHistory",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 20, 15, 59, 30, 338, DateTimeKind.Local).AddTicks(1609),
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValue: new DateTime(2026, 1, 29, 20, 15, 24, 450, DateTimeKind.Local).AddTicks(6007));

            migrationBuilder.AddColumn<string>(
                name: "FileStarter",
                table: "Devices",
                type: "character varying(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortNum",
                table: "Devices",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Protocol",
                table: "Devices",
                type: "character varying(10)",
                unicode: false,
                maxLength: 10,
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileStarter",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "PortNum",
                table: "Devices");

            migrationBuilder.DropColumn(
                name: "Protocol",
                table: "Devices");

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "VersionHistory",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 29, 20, 15, 24, 450, DateTimeKind.Local).AddTicks(6007),
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValue: new DateTime(2026, 2, 20, 15, 59, 30, 338, DateTimeKind.Local).AddTicks(1609));
        }
    }
}
