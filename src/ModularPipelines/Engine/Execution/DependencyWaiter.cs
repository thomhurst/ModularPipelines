using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using ModularPipelines.Exceptions;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

using ModularPipelines.Generated;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for waiting for module dependencies to complete before execution.
/// </summary>
internal class DependencyWaiter : IDependencyWaiter
{
    /// <inheritdoc />
    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Generated runtime metadata handles statically known modules; MakeGenericType is the documented fallback for dynamic modules.")]
    public async Task WaitForDependenciesAsync(
        ModuleState moduleState,
        IModuleScheduler scheduler,
        IServiceProvider scopedServiceProvider,
        CancellationToken workerCancellationToken)
    {
        foreach (var (dependencyType, optional) in moduleState.Dependencies)
        {
            var dependencyTask = scheduler.GetModuleCompletionTask(dependencyType);

            if (dependencyTask != null)
            {
                try
                {
                    await dependencyTask.ConfigureAwait(false);
                }
                catch (Exception e) when (moduleState.Module.Configuration.AlwaysRun)
                {
                    var depLogger = GeneratedModuleMetadata.TryGetRuntime(
                        moduleState.ModuleType,
                        out var runtime)
                            ? runtime.GetLogger(scopedServiceProvider)
                            : (IModuleLogger) scopedServiceProvider.GetRequiredService(
                                typeof(ModuleLogger<>).MakeGenericType(moduleState.ModuleType));
                    depLogger.LogIgnoredDependencyFailure(e);
                }
                catch (Exception e) when (
                    !WorkerCancellationClassifier.IsExpected(e, workerCancellationToken))
                {
                    var dependency = scheduler.GetModuleState(dependencyType)?.Module;
                    if (dependency is null)
                    {
                        throw;
                    }

                    throw new DependencyFailedException(e, dependency);
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
}
