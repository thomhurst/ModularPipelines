using System.Diagnostics.CodeAnalysis;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using ModularPipelines.Exceptions;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;
using ModularPipelines.Options;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for waiting for module dependencies to complete before execution.
/// </summary>
internal class DependencyWaiter : IDependencyWaiter
{
    private readonly ISecondaryExceptionContainer _secondaryExceptionContainer;
    private readonly IOptions<PipelineOptions> _pipelineOptions;

    public DependencyWaiter(
        ISecondaryExceptionContainer secondaryExceptionContainer,
        IOptions<PipelineOptions> pipelineOptions)
    {
        _secondaryExceptionContainer = secondaryExceptionContainer;
        _pipelineOptions = pipelineOptions;
    }

    /// <inheritdoc />
    [UnconditionalSuppressMessage(
        "AOT",
        "IL3050",
        Justification = "Generated runtime metadata handles statically known modules; MakeGenericType is the documented fallback for dynamic modules.")]
    public async Task WaitForDependenciesAsync(ModuleState moduleState, IModuleScheduler scheduler, IServiceProvider scopedServiceProvider)
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
                catch (Exception e) when (moduleState.Module.ModuleRunType == ModuleRunType.AlwaysRun)
                {
                    var depLogger = GeneratedModuleMetadata.TryGetRuntime(
                        moduleState.ModuleType,
                        out var runtime)
                            ? runtime.GetLogger(scopedServiceProvider)
                            : (IModuleLogger) scopedServiceProvider.GetRequiredService(
                                typeof(ModuleLogger<>).MakeGenericType(moduleState.ModuleType));
                    _secondaryExceptionContainer.RegisterException(new AlwaysRunPostponedException(
                        $"{dependencyType.Name} threw an exception when {moduleState.ModuleType.Name} was waiting for it as a dependency",
                        e));
                    depLogger.LogError(e, "Ignoring Exception due to 'AlwaysRun' set");
                }
                catch (Exception e) when (
                    e is not OperationCanceledException
                    && _pipelineOptions.Value.ExecutionMode == ExecutionMode.WaitForAllModules)
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
