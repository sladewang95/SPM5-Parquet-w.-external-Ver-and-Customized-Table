#region license
// Copyright 2025 Utah Departement of Transportation
// for ConfigApi - Utah.Udot.Atspm.ConfigApi.Controllers/DeviceConfigurationController.cs
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

using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.Extensions.Logging;
using System.Reflection;
using Utah.Udot.Atspm.Data.Models;
using Utah.Udot.Atspm.Data.Models.EventLogModels;
using Utah.Udot.Atspm.Repositories.ConfigurationRepositories;
using Utah.Udot.Atspm.Services;
using static Microsoft.AspNetCore.Http.StatusCodes;
using static Microsoft.AspNetCore.OData.Query.AllowedQueryOptions;

namespace Utah.Udot.Atspm.ConfigApi.Controllers
{
    /// <summary>
    /// Device configuration controller
    /// </summary>
    [ApiVersion(1.0)]
    public class DeviceConfigurationController : LocationPolicyControllerBase<DeviceConfiguration, int>
    {
        private readonly IDeviceConfigurationRepository _repository;
        private readonly ILogger<DeviceConfigurationController> _logger;

        /// <inheritdoc/>
        public DeviceConfigurationController(IDeviceConfigurationRepository repository, ILogger<DeviceConfigurationController> logger) : base(repository)
        {
            _repository = repository;
            _logger = logger;
        }

        #region NavigationProperties

        /// <summary>
        /// <see cref="Device"/> navigation property action
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [EnableQuery(AllowedQueryOptions = Count | Filter | Select | OrderBy | Top | Skip)]
        [ProducesResponseType(Status200OK)]
        [ProducesResponseType(Status404NotFound)]
        [ProducesResponseType(Status400BadRequest)]
        public ActionResult<IEnumerable<Device>> GetDevices([FromRoute] int key)
        {
            return GetNavigationProperty<IEnumerable<Device>>(key);
        }

        #endregion

        #region Actions

        #endregion

        #region Functions

        /// <summary>
        /// Gets all implementations of <see cref="IEventLogDecoder"/>
        /// that can be assigned to <see cref="DeviceConfiguration"/> for decoding <see cref="EventLogModelBase"/> derived types.
        /// </summary>
        /// <returns>List of <see cref="IEventLogDecoder"/> implementations</returns>
        [HttpGet]
        [EnableQuery(AllowedQueryOptions = Count | Filter | Select | OrderBy | Top | Skip)]
        [ProducesResponseType(typeof(IEnumerable<string>), Status200OK)]
        public IActionResult GetEventLogDecoders()
        {
            // Scan all assemblies for IEventLogDecoder implementations
            // Use try-catch to gracefully handle problematic assemblies (e.g., Microsoft.Data.SqlClient)
            // that throw ReflectionTypeLoadException on Linux/Docker due to memory alignment issues
            var result = AppDomain
                .CurrentDomain
                .GetAssemblies()
                .Where(a => !a.IsDynamic) // Skip dynamic assemblies
                .SelectMany(a =>
                {
                    try
                    {
                        return a.GetTypes();
                    }
                    catch (ReflectionTypeLoadException ex)
                    {
                        // Log assemblies that fail to load (e.g., Microsoft.Data.SqlClient on Linux)
                        _logger.LogWarning("Skipped assembly '{AssemblyName}' due to ReflectionTypeLoadException: {Message}. LoaderExceptions: {LoaderExceptions}", 
                            a.FullName, 
                            ex.Message,
                            string.Join("; ", ex.LoaderExceptions?.Take(3).Select(e => e?.Message ?? "null") ?? Array.Empty<string>()));
                        return Array.Empty<Type>();
                    }
                    catch (Exception ex)
                    {
                        // Log any other problematic assemblies
                        _logger.LogWarning("Skipped assembly '{AssemblyName}' due to exception: {ExceptionType} - {Message}", a.FullName, ex.GetType().Name, ex.Message);
                        return Array.Empty<Type>();
                    }
                })
                .Where(w => !w.IsAbstract)
                .Where(w => !w.IsInterface)
                .Where(w => typeof(IEventLogDecoder).IsAssignableFrom(w))
                .Select(s => s.Name)
                .OrderBy(s => s)
                .ToList();

            return Ok(result);
        }

        #endregion
    }
}
