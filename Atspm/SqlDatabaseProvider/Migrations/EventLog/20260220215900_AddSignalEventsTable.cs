using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Utah.Udot.SqlDatabaseProvider.Migrations.EventLog
{
    /// <inheritdoc />
    public partial class AddSignalEventsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SignalEvents",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    EventCode = table.Column<short>(type: "smallint", nullable: false),
                    EventParam = table.Column<short>(type: "smallint", nullable: false),
                    DeviceId = table.Column<int>(type: "int", nullable: false),
                    LocationIdentifier = table.Column<string>(type: "varchar(10)", unicode: false, maxLength: 10, nullable: false),
                    Timestamp = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SignalEvents", x => x.Id);
                },
                comment: "Signal event data");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SignalEvents");
        }
    }
}
