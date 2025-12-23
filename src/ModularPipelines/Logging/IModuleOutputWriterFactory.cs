using Microsoft.Extensions.Logging;

namespace ModularPipelines.Logging;

/// <summary>
/// Factory for creating scoped <see cref="IModuleOutputWriter"/> instances.
/// </summary>
internal interface IModuleOutputWriterFactory
{
    /// <summary>
    /// Creates a new output writer for the specified module.
    /// </summary>
    /// <param name="moduleName">The module name for section headers.</param>
    /// <param name="logger">Optional logger to use for flushing log events. If not provided, uses ILoggerFactory.</param>
    /// <returns>A new output writer instance.</returns>
    ModuleOutputWriter Create(string moduleName, ILogger? logger = null);
}
