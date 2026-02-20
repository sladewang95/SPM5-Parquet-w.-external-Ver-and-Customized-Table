using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utah.Udot.SqlDatabaseProvider.Migrations.Config
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
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 2, 20, 15, 58, 25, 681, DateTimeKind.Local).AddTicks(7893),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 1, 29, 20, 27, 20, 698, DateTimeKind.Local).AddTicks(477));

            migrationBuilder.AddColumn<string>(
                name: "FileStarter",
                table: "Devices",
                type: "varchar(50)",
                unicode: false,
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PortNum",
                table: "Devices",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Protocol",
                table: "Devices",
                type: "varchar(10)",
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
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 29, 20, 27, 20, 698, DateTimeKind.Local).AddTicks(477),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldDefaultValue: new DateTime(2026, 2, 20, 15, 58, 25, 681, DateTimeKind.Local).AddTicks(7893));
        }
    }
}
