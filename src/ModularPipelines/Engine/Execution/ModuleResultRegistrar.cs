using Microsoft.Extensions.Logging;
using ModularPipelines.Engine.Executors;
using ModularPipelines.Generated;
using ModularPipelines.Helpers;
using ModularPipelines.Logging;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Engine.Execution;

/// <summary>
/// Responsible for registering module execution results.
/// </summary>
internal class ModuleResultRegistrar : IModuleResultRegistrar
{
    private readonly IModuleResultRegistry _resultRegistry;
    private readonly ILogger<ModuleResultRegistrar> _logger;

    public ModuleResultRegistrar(
        IModuleResultRegistry resultRegistry,
        ILogger<ModuleResultRegistrar> logger)
    {
        _resultRegistry = resultRegistry;
        _logger = logger;
    }

    /// <inheritdoc />
    public void RegisterTerminatedResult(IModule module, Type moduleType, Exception exception)
    {
        RegisterFailureResult(module, moduleType, exception, Enums.ModuleStatus.Cancelled);
    }

    /// <inheritdoc />
    public void RegisterDependencyFailedResult(IModule module, Type moduleType, Exception exception)
    {
        RegisterFailureResult(module, moduleType, exception, Enums.ModuleStatus.DependencyFailed);
    }

    private void RegisterFailureResult(
        IModule module,
        Type moduleType,
        Exception exception,
        Enums.ModuleStatus status)
    {
        var resultType = module.ResultType;

        var executionContext = ExecutionContextFactory.Create(module, moduleType);
        executionContext.Status = status;
        executionContext.Exception = exception;

        var hasGeneratedRuntime = GeneratedModuleMetadata.TryGetRuntime(moduleType, out var runtime);
        var result = hasGeneratedRuntime
            ? runtime.CreateFailure(exception, executionContext)
            : ModuleResultFactory.CreateException(resultType, exception, executionContext);

        _resultRegistry.RegisterResult(moduleType, result);

        if (hasGeneratedRuntime)
        {
            runtime.SetCompletionSource(module, result);
        }
        else
        {
            CompletionSourceSetterCache.GetOrCreate(resultType)(module, result);
        }
    }

    /// <inheritdoc />
    public void RegisterTerminatedResultsForCancelledModules(IReadOnlyList<IModule> modules, Exception exception)
    {
        foreach (var module in modules)
        {
            // AlwaysRun modules may still execute after scheduler cancellation.
            // Their final execution path must own both registry and typed-task completion.
            if (module.Configuration.AlwaysRun)
            {
                continue;
            }

            var moduleType = module.GetType();

            // Check if a result was already registered for this module
            if (_resultRegistry.GetResult(moduleType) != null)
            {
                continue;
            }

            _logger.LogDebug(
                "Registering Cancelled result for cancelled module {ModuleName}",
                moduleType.Name);

            RegisterTerminatedResult(module, moduleType, exception);
        }
    }
}
