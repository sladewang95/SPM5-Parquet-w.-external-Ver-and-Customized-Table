using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Utah.Udot.ATSPM.PostgreSQLDatabaseProvider.Migrations
{
    /// <inheritdoc />
    public partial class SeedProductsDeviceConfigsAndDetectionTypes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "VersionHistory",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 29, 20, 15, 24, 450, DateTimeKind.Local).AddTicks(6007),
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValue: new DateTime(2026, 1, 29, 20, 1, 9, 851, DateTimeKind.Local).AddTicks(7410));

            // Seed Products
            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "Created", "CreatedBy", "Manufacturer", "Model", "Modified", "ModifiedBy", "Notes", "WebPage" },
                values: new object[,]
                {
                    { 1, null, null, "Econolite", "ASC3", null, null, null, null },
                    { 2, null, null, "QFree", "Cobalt", null, null, null, null },
                    { 3, null, null, "Econolite", "2070", null, null, null, null },
                    { 4, null, null, "QFree", "MaxTime", null, null, null, null },
                    { 5, null, null, "Trafficware", "Trafficware", null, null, null, null },
                    { 6, null, null, "Siemens", "SEPAC", null, null, null, null },
                    { 7, null, null, "McCain", "ATC EX", null, null, null, null },
                    { 8, null, null, "Peek", "Peek", null, null, null, null },
                    { 9, null, null, "Econolite", "EOS", null, null, null, null }
                });

            // Seed DeviceConfigurations
            migrationBuilder.InsertData(
                table: "DeviceConfigurations",
                columns: new[] { "Id", "ConnectionProperties", "ConnectionTimeout", "Created", "CreatedBy", "Decoders", "Description", "LoggingOffset", "Modified", "ModifiedBy", "Notes", "OperationTimeout", "Password", "Path", "Port", "ProductId", "Protocol", "Query", "UserName" },
                values: new object[,]
                {
                    { 1, null, 2000, null, null, "[]", "ASC3", 0, null, null, null, 2000, "ecpi2ecpi", "/set1", 22, 1, "Sftp", "[]", "econolite" },
                    { 2, null, 2000, null, null, "[]", "Cobalt", 0, null, null, null, 2000, "ecpi2ecpi", "/set1", 22, 2, "Sftp", "[]", "econolite" },
                    { 3, null, 2000, null, null, "[]", "ASC3 - 2070", 0, null, null, null, 2000, "ecpi2ecpi", "/set1", 22, 3, "Sftp", "[]", "econolite" },
                    { 4, null, 2000, null, null, "[]", "MaxTime", 0, null, null, null, 2000, "none", "v1/asclog/xml/full", 80, 4, "Http", "[]", "none" },
                    { 5, null, 2000, null, null, "[]", "Trafficware", 0, null, null, null, 2000, "none", "none", 22, 5, "Http", "[]", "none" },
                    { 6, null, 2000, null, null, "[]", "Siemens SEPAC", 0, null, null, null, 2000, "$adm^kon2", "/mnt/sd", 22, 6, "Sftp", "[]", "admin" },
                    { 7, null, 2000, null, null, "[]", "McCain ATC EX", 0, null, null, null, 2000, "root", "/mnt/rdhi/ResData", 22, 7, "Sftp", "[]", "root" },
                    { 8, null, 2000, null, null, "[]", "Peek", 0, null, null, null, 2000, "PeekAtc", "mn't/sram/cuLLogging", 22, 8, "Sftp", "[]", "atc" },
                    { 9, null, 2000, null, null, "[]", "EOS", 0, null, null, null, 2000, "ecpi2ecpi", "/set1", 22, 9, "Sftp", "[]", "econolite" }
                });

            // Seed DetectionTypeMeasureType join table
            migrationBuilder.InsertData(
                table: "DetectionTypeMeasureType",
                columns: new[] { "DetectionTypesId", "MeasureTypesId" },
                values: new object[,]
                {
                    { 1, 1 }, { 1, 2 }, { 1, 3 }, { 1, 4 }, { 1, 14 }, { 1, 15 }, { 1, 17 }, { 1, 31 },
                    { 2, 6 }, { 2, 7 }, { 2, 8 }, { 2, 9 }, { 2, 13 }, { 2, 32 },
                    { 3, 10 },
                    { 4, 5 }, { 4, 7 }, { 4, 31 }, { 4, 36 },
                    { 5, 11 },
                    { 6, 12 }, { 6, 31 }, { 6, 32 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 1, 1 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 1, 2 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 1, 3 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 1, 4 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 1, 14 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 1, 15 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 1, 17 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 1, 31 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 2, 6 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 2, 7 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 2, 8 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 2, 9 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 2, 13 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 2, 32 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 3, 10 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 4, 5 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 4, 7 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 4, 31 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 4, 36 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 5, 11 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 6, 12 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 6, 31 });
            
            migrationBuilder.DeleteData(
                table: "DetectionTypeMeasureType",
                keyColumns: new[] { "DetectionTypesId", "MeasureTypesId" },
                keyValues: new object[] { 6, 32 });

            migrationBuilder.DeleteData(
                table: "DeviceConfigurations",
                keyColumn: "Id",
                keyValue: 1);
            
            migrationBuilder.DeleteData(
                table: "DeviceConfigurations",
                keyColumn: "Id",
                keyValue: 2);
            
            migrationBuilder.DeleteData(
                table: "DeviceConfigurations",
                keyColumn: "Id",
                keyValue: 3);
            
            migrationBuilder.DeleteData(
                table: "DeviceConfigurations",
                keyColumn: "Id",
                keyValue: 4);
            
            migrationBuilder.DeleteData(
                table: "DeviceConfigurations",
                keyColumn: "Id",
                keyValue: 5);
            
            migrationBuilder.DeleteData(
                table: "DeviceConfigurations",
                keyColumn: "Id",
                keyValue: 6);
            
            migrationBuilder.DeleteData(
                table: "DeviceConfigurations",
                keyColumn: "Id",
                keyValue: 7);
            
            migrationBuilder.DeleteData(
                table: "DeviceConfigurations",
                keyColumn: "Id",
                keyValue: 8);
            
            migrationBuilder.DeleteData(
                table: "DeviceConfigurations",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 1);
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 2);
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 3);
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 4);
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 5);
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 6);
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 7);
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 8);
            
            migrationBuilder.DeleteData(
                table: "Products",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.AlterColumn<DateTime>(
                name: "Date",
                table: "VersionHistory",
                type: "timestamp",
                nullable: false,
                defaultValue: new DateTime(2026, 1, 29, 20, 1, 9, 851, DateTimeKind.Local).AddTicks(7410),
                oldClrType: typeof(DateTime),
                oldType: "timestamp",
                oldDefaultValue: new DateTime(2026, 1, 29, 20, 15, 24, 450, DateTimeKind.Local).AddTicks(6007));
        }
    }
}
