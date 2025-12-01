using System.Text.Json.Serialization;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Modules.Behaviors;

namespace ModularPipelines.Modules;

/// <summary>
/// Marker interface for all modules, enabling non-generic operations.
/// </summary>
public interface IModule
{
    /// <summary>
    /// Gets the result type of this module.
    /// </summary>
    [JsonIgnore]
    Type ResultType { get; }

    /// <summary>
    /// Gets whether this module should always run, even when the pipeline fails.
    /// </summary>
    ModuleRunType ModuleRunType => this is IAlwaysRun ? ModuleRunType.AlwaysRun : ModuleRunType.OnSuccessfulDependencies;
}

/// <summary>
/// Core interface for pipeline modules. Implement this interface to define a module's execution logic.
/// </summary>
/// <typeparam name="T">The type of result returned by the module.</typeparam>
/// <remarks>
/// Modules can optionally implement behavior interfaces to customize execution:
/// <list type="bullet">
/// <item><see cref="Behaviors.ISkippable"/> - Define skip conditions</item>
/// <item><see cref="Behaviors.ITimeoutable"/> - Set execution timeout</item>
/// <item><see cref="Behaviors.IRetryable{T}"/> - Configure retry policy</item>
/// <item><see cref="Behaviors.IIgnoreFailures"/> - Handle failures gracefully</item>
/// <item><see cref="Behaviors.IHookable"/> - Add before/after execution hooks</item>
/// <item><see cref="Behaviors.IHistoryAware"/> - Enable result caching</item>
/// <item><see cref="Behaviors.IAlwaysRun"/> - Run even when pipeline fails</item>
/// </list>
/// </remarks>
public interface IModule<T> : IModule
{
    /// <inheritdoc />
    Type IModule.ResultType => typeof(T);

    /// <summary>
    /// Executes the module's core logic.
    /// </summary>
    /// <param name="context">The module context providing access to pipeline services.</param>
    /// <param name="cancellationToken">A token that will be cancelled if the pipeline fails or the module times out.</param>
    /// <returns>The result of the module execution, or null.</returns>
    Task<T?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken);
}
