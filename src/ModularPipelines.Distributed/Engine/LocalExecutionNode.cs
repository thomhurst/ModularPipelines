using Microsoft.Extensions.Logging;
using ModularPipelines.Distributed.Abstractions;
using ModularPipelines.Models;
using ModularPipelines.Modules;

namespace ModularPipelines.Distributed.Engine;

/// <summary>
/// Executes modules locally on the current node (orchestrator or standalone).
/// </summary>
/// <remarks>
/// This node executes modules in the local process. When used in distributed mode,
/// the module's dependencies should already be available in the dependency results parameter.
/// This implementation assumes the module has been properly initialized before execution.
/// </remarks>
internal sealed class LocalExecutionNode : IExecutionNode
{
    private readonly ILogger<LocalExecutionNode> _logger;
    private int _currentLoad;

    public LocalExecutionNode(ILogger<LocalExecutionNode> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        NodeId = $"local-{Environment.MachineName}";
    }

    /// <inheritdoc />
    public string NodeId { get; }

    /// <inheritdoc />
    public bool CanExecute(ModuleBase module)
    {
        // Local node can execute any module
        return true;
    }

    /// <inheritdoc />
    public async Task<IModuleResult> ExecuteAsync(
        ModuleBase module,
        IReadOnlyDictionary<Type, IModuleResult> dependencyResults,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(module);
        ArgumentNullException.ThrowIfNull(dependencyResults);

        Interlocked.Increment(ref _currentLoad);

        try
        {
            _logger.LogInformation(
                "Executing module {ModuleType} locally on {NodeId}",
                module.GetType().Name,
                NodeId);

            // The module should already be initialized with context by the caller
            // Dependencies are provided via dependencyResults parameter

            // Get the module result - this will trigger execution if not already started
            var result = await module.GetModuleResult();

            _logger.LogInformation(
                "Module {ModuleType} completed locally. Status: {Status}, Duration: {Duration}",
                module.GetType().Name,
                result.ModuleStatus,
                result.ModuleDuration);

            return result;
        }
        catch (Exception ex)
        {
            _logger.LogError(
                ex,
                "Failed to execute module {ModuleType} locally",
                module.GetType().Name);
            throw;
        }
        finally
        {
            Interlocked.Decrement(ref _currentLoad);
        }
    }

    /// <inheritdoc />
    public int GetCurrentLoad()
    {
        return Interlocked.CompareExchange(ref _currentLoad, 0, 0);
    }
}
