#region license
// Copyright 2025 Utah Departement of Transportation
// for Data - Utah.Udot.Atspm.Data.Configuration/DetectionTypeConfiguration.cs
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
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using Utah.Udot.Atspm.Data.Enums;

namespace Utah.Udot.Atspm.Data.Configuration
{
    /// <summary>
    /// Detection type configuration
    /// </summary>
    public class DetectionTypeConfiguration : IEntityTypeConfiguration<DetectionType>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<DetectionType> builder)
        {
            builder.ToTable(t => t.HasComment("Detector Types"));

            builder.Property(e => e.Id)
                .ValueGeneratedNever();

            builder.Property(e => e.Abbreviation).HasMaxLength(5);

            builder.Property(e => e.Description).HasMaxLength(128).IsRequired();

            builder.HasData(typeof(DetectionTypes).GetFields().Where(t => t.FieldType == typeof(DetectionTypes)).Select(s => new DetectionType()
            {
                Id = (DetectionTypes)s.GetValue(s),
                Description = s.GetCustomAttribute<DisplayAttribute>().Name,
                Abbreviation = s.GetValue(s).ToString(),
                DisplayOrder = s.GetCustomAttribute<DisplayAttribute>().Order
            }));

            // Configure many-to-many relationship and seed join table data
            builder.HasMany(d => d.MeasureTypes)
                .WithMany(m => m.DetectionTypes)
                .UsingEntity<Dictionary<string, object>>(
                    "DetectionTypeMeasureType",
                    j => j.HasOne<MeasureType>().WithMany().HasForeignKey("MeasureTypesId"),
                    j => j.HasOne<DetectionType>().WithMany().HasForeignKey("DetectionTypesId"),
                    j =>
                    {
                        j.HasKey("DetectionTypesId", "MeasureTypesId");
                        
                        // Seed the join table relationships
                        j.HasData(
                            // Basic (1) -> PPT(1), SM(2), PedD(3), PD(4), PSR(14), PS(15), TAA(17), LTGA(31)
                            new { DetectionTypesId = DetectionTypes.B, MeasureTypesId = 1 },
                            new { DetectionTypesId = DetectionTypes.B, MeasureTypesId = 2 },
                            new { DetectionTypesId = DetectionTypes.B, MeasureTypesId = 3 },
                            new { DetectionTypesId = DetectionTypes.B, MeasureTypesId = 4 },
                            new { DetectionTypesId = DetectionTypes.B, MeasureTypesId = 14 },
                            new { DetectionTypesId = DetectionTypes.B, MeasureTypesId = 15 },
                            new { DetectionTypesId = DetectionTypes.B, MeasureTypesId = 17 },
                            new { DetectionTypesId = DetectionTypes.B, MeasureTypesId = 31 },
                            
                            // Advanced Count (2) -> PCD(6), AV(7), AD(8), AoR(9), LP(13), WT(32)
                            new { DetectionTypesId = DetectionTypes.AC, MeasureTypesId = 6 },
                            new { DetectionTypesId = DetectionTypes.AC, MeasureTypesId = 7 },
                            new { DetectionTypesId = DetectionTypes.AC, MeasureTypesId = 8 },
                            new { DetectionTypesId = DetectionTypes.AC, MeasureTypesId = 9 },
                            new { DetectionTypesId = DetectionTypes.AC, MeasureTypesId = 13 },
                            new { DetectionTypesId = DetectionTypes.AC, MeasureTypesId = 32 },
                            
                            // Advanced Speed (3) -> Speed(10)
                            new { DetectionTypesId = DetectionTypes.AS, MeasureTypesId = 10 },
                            
                            // Lane-by-lane Count (4) -> TMC(5), AV(7), LTGA(31), GTU(36)
                            new { DetectionTypesId = DetectionTypes.LLC, MeasureTypesId = 5 },
                            new { DetectionTypesId = DetectionTypes.LLC, MeasureTypesId = 7 },
                            new { DetectionTypesId = DetectionTypes.LLC, MeasureTypesId = 31 },
                            new { DetectionTypesId = DetectionTypes.LLC, MeasureTypesId = 36 },
                            
                            // Lane-by-lane Speed (5) -> YRA(11)
                            new { DetectionTypesId = DetectionTypes.LLS, MeasureTypesId = 11 },
                            
                            // Stop Bar Presence (6) -> SF(12), LTGA(31), WT(32)
                            new { DetectionTypesId = DetectionTypes.SBP, MeasureTypesId = 12 },
                            new { DetectionTypesId = DetectionTypes.SBP, MeasureTypesId = 31 },
                            new { DetectionTypesId = DetectionTypes.SBP, MeasureTypesId = 32 }
                        );
                    });
        }
    }
}
