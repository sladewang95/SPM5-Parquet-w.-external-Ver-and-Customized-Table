#region license
// Copyright 2025 Utah Departement of Transportation
// for Data - Utah.Udot.Atspm.Data.Models.EventLogModels/SignalEvent.cs
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

#nullable disable

namespace Utah.Udot.Atspm.Data.Models.EventLogModels
{
    /// <summary>
    /// Signal event data table
    /// </summary>
    public class SignalEvent
    {
        /// <summary>
        /// Primary key identifier
        /// </summary>
        public long Id { get; set; }

        /// <summary>
        /// Event code
        /// </summary>
        public short EventCode { get; set; }

        /// <summary>
        /// Event parameter
        /// </summary>
        public short EventParam { get; set; }

        /// <summary>
        /// Device identifier
        /// </summary>
        public int DeviceId { get; set; }

        /// <summary>
        /// Location identifier
        /// </summary>
        public string LocationIdentifier { get; set; }

        /// <summary>
        /// Event timestamp
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}
