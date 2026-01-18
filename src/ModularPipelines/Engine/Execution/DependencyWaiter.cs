using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Engine.Dependencies;
using ModularPipelines.Exceptions;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for waiting for module dependencies to complete before execution.
/// </summary>
internal class DependencyWaiter : IDependencyWaiter
{
    private readonly ISecondaryExceptionContainer _secondaryExceptionContainer;

    public DependencyWaiter(ISecondaryExceptionContainer secondaryExceptionContainer)
    {
        _secondaryExceptionContainer = secondaryExceptionContainer;
    }

    /// <inheritdoc />
    public async Task WaitForDependenciesAsync(ModuleState moduleState, IModuleScheduler scheduler, IServiceProvider scopedServiceProvider)
    {
        // Get both attribute-based and programmatic dependencies
        var dependencies = GetAllDependencies(moduleState);

        foreach (var (dependencyType, optional) in dependencies)
        {
            var dependencyTask = scheduler.GetModuleCompletionTask(dependencyType);

            if (dependencyTask != null)
            {
                try
                {
                    await dependencyTask.ConfigureAwait(false);
                }
                catch (Exception e) when (moduleState.Module.ModuleRunType == ModuleRunType.AlwaysRun)
                {
                    var loggerType = typeof(ModuleLogger<>).MakeGenericType(moduleState.ModuleType);
                    var depLogger = (IModuleLogger)scopedServiceProvider.GetRequiredService(loggerType);
                    _secondaryExceptionContainer.RegisterException(new AlwaysRunPostponedException(
                        $"{dependencyType.Name} threw an exception when {moduleState.ModuleType.Name} was waiting for it as a dependency",
                        e));
                    depLogger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");
                }
            }
            else if (!optional)
            {
                var message = $"Module '{moduleState.ModuleType.Name}' requires '{dependencyType.Name}', " +
                              $"but '{dependencyType.Name}' has not been registered and could not be auto-registered.\n\n" +
                              $"Suggestions:\n" +
                              $"  1. Add '.AddModule<{dependencyType.Name}>()' to your pipeline configuration\n" +
                              $"  2. Use '[DependsOn<{dependencyType.Name}>(Optional = true)]' if this dependency is optional";
                throw new ModuleNotRegisteredException(message, null);
            }
        }
    }

    /// <summary>
    /// Gets all dependencies for a module, combining attribute-based and programmatic dependencies.
    /// </summary>
    private static IEnumerable<(Type DependencyType, bool Optional)> GetAllDependencies(ModuleState moduleState)
    {
        // Attribute-based dependencies
        foreach (var dep in ModuleDependencyResolver.GetDependencies(moduleState.ModuleType))
        {
            yield return dep;
        }

        // Programmatic dependencies from DeclareDependencies method
        foreach (var dep in ModuleDependencyResolver.GetProgrammaticDependencies(moduleState.Module))
        {
            yield return dep;
        }
    }
}
