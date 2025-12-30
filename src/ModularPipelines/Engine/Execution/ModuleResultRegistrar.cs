using Microsoft.Extensions.Logging;
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
        var resultType = module.ResultType;

        // Create execution context with PipelineTerminated status using compiled delegate factory
        var executionContext = ExecutionContextFactory.Create(module, moduleType);
        executionContext.Status = Enums.Status.PipelineTerminated;
        executionContext.Exception = exception;

        // Create ModuleResult<T> with the exception using compiled delegate factory
        var result = ModuleResultFactory.CreateException(resultType, exception, executionContext);

        _resultRegistry.RegisterResult(moduleType, result);
    }

    /// <inheritdoc />
    public void RegisterTerminatedResultsForCancelledModules(IReadOnlyList<IModule> modules, Exception exception)
    {
        foreach (var module in modules)
        {
            var moduleType = module.GetType();

            // Check if a result was already registered for this module
            if (_resultRegistry.GetResult(moduleType) != null)
            {
                continue;
            }

            _logger.LogDebug(
                "Registering PipelineTerminated result for cancelled module {ModuleName}",
                MarkupFormatter.FormatModuleName(moduleType.Name));

            RegisterTerminatedResult(module, moduleType, exception);
        }
    }
}
