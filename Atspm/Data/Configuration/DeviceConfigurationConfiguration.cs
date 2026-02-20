#region license
// Copyright 2025 Utah Departement of Transportation
// for Data - Utah.Udot.Atspm.Data.Configuration/DeviceConfigurationConfiguration.cs
// 
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
// 
// http://www.apache.org/licenses/LICENSE-2.
// 
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.
#endregion

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Utah.Udot.Atspm.Data.Enums;

namespace Utah.Udot.Atspm.Data.Configuration
{
    /// <summary>
    /// Device configuration configuration
    /// </summary>
    public class DeviceConfigurationConfiguration : IEntityTypeConfiguration<DeviceConfiguration>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<DeviceConfiguration> builder)
        {
            builder.ToTable(t => t.HasComment("Device Configurations"));

            builder.Property(e => e.Description)
                .IsRequired()
                .HasMaxLength(128);

            builder.Property(e => e.Notes)
                .HasMaxLength(512);

            builder.Property(e => e.Path)
                .HasMaxLength(256);

            builder.Property(e => e.UserName)
                .HasMaxLength(128);

            builder.Property(e => e.Password)
                .HasMaxLength(128);

            // Seed default device configurations
            builder.HasData(
                new DeviceConfiguration
                {
                    Id = 1,
                    ProductId = 1,
                    Description = "ASC3",
                    Protocol = TransportProtocols.Sftp,
                    Port = 22,
                    Path = "/set1",
                    Query = new string[] { },
                    Decoders = new string[] { },
                    ConnectionTimeout = 2000,
                    OperationTimeout = 2000,
                    LoggingOffset = 0,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                },
                new DeviceConfiguration
                {
                    Id = 2,
                    ProductId = 2,
                    Description = "Cobalt",
                    Protocol = TransportProtocols.Sftp,
                    Port = 22,
                    Path = "/set1",
                    Query = new string[] { },
                    Decoders = new string[] { },
                    ConnectionTimeout = 2000,
                    OperationTimeout = 2000,
                    LoggingOffset = 0,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                },
                new DeviceConfiguration
                {
                    Id = 3,
                    ProductId = 3,
                    Description = "ASC3 - 2070",
                    Protocol = TransportProtocols.Sftp,
                    Port = 22,
                    Path = "/set1",
                    Query = new string[] { },
                    Decoders = new string[] { },
                    ConnectionTimeout = 2000,
                    OperationTimeout = 2000,
                    LoggingOffset = 0,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                },
                new DeviceConfiguration
                {
                    Id = 4,
                    ProductId = 4,
                    Description = "MaxTime",
                    Protocol = TransportProtocols.Http,
                    Port = 80,
                    Path = "v1/asclog/xml/full",
                    Query = new string[] { },
                    Decoders = new string[] { },
                    ConnectionTimeout = 2000,
                    OperationTimeout = 2000,
                    LoggingOffset = 0,
                    UserName = "none",
                    Password = "none"
                },
                new DeviceConfiguration
                {
                    Id = 5,
                    ProductId = 5,
                    Description = "Trafficware",
                    Protocol = TransportProtocols.Http,
                    Port = 22,
                    Path = "none",
                    Query = new string[] { },
                    Decoders = new string[] { },
                    ConnectionTimeout = 2000,
                    OperationTimeout = 2000,
                    LoggingOffset = 0,
                    UserName = "none",
                    Password = "none"
                },
                new DeviceConfiguration
                {
                    Id = 6,
                    ProductId = 6,
                    Description = "Siemens SEPAC",
                    Protocol = TransportProtocols.Sftp,
                    Port = 22,
                    Path = "/mnt/sd",
                    Query = new string[] { },
                    Decoders = new string[] { },
                    ConnectionTimeout = 2000,
                    OperationTimeout = 2000,
                    LoggingOffset = 0,
                    UserName = "admin",
                    Password = "$adm^kon2"
                },
                new DeviceConfiguration
                {
                    Id = 7,
                    ProductId = 7,
                    Description = "McCain ATC EX",
                    Protocol = TransportProtocols.Sftp,
                    Port = 22,
                    Path = "/mnt/rdhi/ResData",
                    Query = new string[] { },
                    Decoders = new string[] { },
                    ConnectionTimeout = 2000,
                    OperationTimeout = 2000,
                    LoggingOffset = 0,
                    UserName = "root",
                    Password = "root"
                },
                new DeviceConfiguration
                {
                    Id = 8,
                    ProductId = 8,
                    Description = "Peek",
                    Protocol = TransportProtocols.Sftp,
                    Port = 22,
                    Path = "mn't/sram/cuLLogging",
                    Query = new string[] { },
                    Decoders = new string[] { },
                    ConnectionTimeout = 2000,
                    OperationTimeout = 2000,
                    LoggingOffset = 0,
                    UserName = "atc",
                    Password = "PeekAtc"
                },
                new DeviceConfiguration
                {
                    Id = 9,
                    ProductId = 9,
                    Description = "EOS",
                    Protocol = TransportProtocols.Sftp,
                    Port = 22,
                    Path = "/set1",
                    Query = new string[] { },
                    Decoders = new string[] { },
                    ConnectionTimeout = 2000,
                    OperationTimeout = 2000,
                    LoggingOffset = 0,
                    UserName = "econolite",
                    Password = "ecpi2ecpi"
                }
            );
        }
    }
}
