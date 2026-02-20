#region license
// Copyright 2025 Utah Departement of Transportation
// for Data - Utah.Udot.Atspm.Data.Configuration/SignalEventConfiguration.cs
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
using Utah.Udot.Atspm.Data.Models.EventLogModels;

namespace Utah.Udot.Atspm.Data.Configuration
{
    /// <summary>
    /// SignalEvent configuration
    /// </summary>
    public class SignalEventConfiguration : IEntityTypeConfiguration<SignalEvent>
    {
        /// <inheritdoc/>
        public void Configure(EntityTypeBuilder<SignalEvent> builder)
        {
            builder.ToTable("SignalEvents", t => t.HasComment("Signal event data"));

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .IsRequired()
                .ValueGeneratedOnAdd();

            builder.Property(e => e.EventCode)
                .IsRequired();

            builder.Property(e => e.EventParam)
                .IsRequired();

            builder.Property(e => e.DeviceId)
                .IsRequired();

            builder.Property(e => e.LocationIdentifier)
                .IsRequired()
                .HasMaxLength(10)
                .IsUnicode(false);

            builder.Property(e => e.Timestamp)
                .IsRequired();
        }
    }
}
