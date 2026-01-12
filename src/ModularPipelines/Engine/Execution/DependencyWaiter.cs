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

        foreach (var (dependencyType, ignoreIfNotRegistered) in dependencies)
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
            else if (!ignoreIfNotRegistered)
            {
                var message = $"Module '{moduleState.ModuleType.Name}' depends on '{dependencyType.Name}', " +
                              $"but '{dependencyType.Name}' has not been registered in the pipeline.\n\n" +
                              $"Suggestions:\n" +
                              $"  1. Add '.AddModule<{dependencyType.Name}>()' to your pipeline configuration before '.AddModule<{moduleState.ModuleType.Name}>()'\n" +
                              $"  2. Use 'deps.DependsOnOptional<{dependencyType.Name}>()' or '[DependsOn<{dependencyType.Name}>(IgnoreIfNotRegistered = true)]' if this dependency is optional\n" +
                              $"  3. Check for typos in the dependency type name\n" +
                              $"  4. Verify that '{dependencyType.Name}' is in a project referenced by your pipeline project";
                throw new ModuleNotRegisteredException(message, null);
            }
        }
    }

    /// <summary>
    /// Gets all dependencies for a module, combining attribute-based and programmatic dependencies.
    /// </summary>
    private static IEnumerable<(Type DependencyType, bool IgnoreIfNotRegistered)> GetAllDependencies(ModuleState moduleState)
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
