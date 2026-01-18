using Microsoft.Extensions.DependencyInjection;
using ModularPipelines.Logging;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine;

/// <summary>
/// Default implementation of <see cref="IModuleActivator"/> that sets AsyncLocal context
/// before module construction.
/// </summary>
/// <remarks>
/// This activator ensures that any logging performed during module construction
/// (in constructors or field initializers) will have the correct module context set.
/// The AsyncLocal context is set before construction and restored afterward to support
/// nested scenarios (though module construction is typically not nested).
/// </remarks>
internal sealed class ModuleActivator : IModuleActivator
{
    /// <inheritdoc />
    public IModule CreateModule(Type moduleType, IServiceProvider serviceProvider)
    {
        // Save previous context (typically null during construction phase)
        var previousType = ModuleLogger.CurrentModuleType.Value;

        // Set AsyncLocal context BEFORE construction so constructors can log with context
        ModuleLogger.CurrentModuleType.Value = moduleType;

        try
        {
            return (IModule)ActivatorUtilities.CreateInstance(serviceProvider, moduleType);
        }
        finally
        {
            // Restore previous context
            ModuleLogger.CurrentModuleType.Value = previousType;
        }
    }
}
