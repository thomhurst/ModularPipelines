using System.Runtime.CompilerServices;
using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

/// <summary>
/// Core interface for pipeline modules. Implement this interface to define a module's execution logic.
/// </summary>
/// <typeparam name="T">The type of result returned by the module.</typeparam>
/// <remarks>
/// <para>
/// Modules can optionally implement behavior interfaces to customize execution:
/// </para>
/// <list type="bullet">
/// <item><see cref="Behaviors.ISkippable"/> - Define skip conditions</item>
/// <item><see cref="Behaviors.ITimeoutable"/> - Set execution timeout</item>
/// <item><see cref="Behaviors.IRetryable{T}"/> - Configure retry policy</item>
/// <item><see cref="Behaviors.IIgnoreFailures"/> - Handle failures gracefully</item>
/// <item><see cref="Behaviors.IHookable"/> - Add before/after execution hooks</item>
/// <item><see cref="Behaviors.IAlwaysRun"/> - Run even when pipeline fails</item>
/// </list>
/// <para>
/// Dependencies can be declared in two ways:
/// </para>
/// <list type="bullet">
/// <item>Statically via <see cref="Attributes.DependsOnAttribute"/> attributes</item>
/// <item>Dynamically by overriding <see cref="DeclareDependencies"/></item>
/// </list>
/// </remarks>
/// <example>
/// <code>
/// public class ApiModule : Module&lt;ApiResult&gt;
/// {
///     protected override void DeclareDependencies(IDependencyDeclaration deps)
///     {
///         deps.DependsOn&lt;DatabaseModule&gt;();
///         deps.DependsOnOptional&lt;CachingModule&gt;();
///         deps.DependsOnIf&lt;MonitoringModule&gt;(Environment.IsProduction);
///     }
///
///     public override async Task&lt;ApiResult?&gt; ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
///     {
///         // Implementation
///     }
/// }
/// </code>
/// </example>
public abstract class Module<T> : IModule
{
    internal TaskCompletionSource<ModuleResult<T?>> CompletionSource { get; } = new();

    /// <inheritdoc />
    Type IModule.ResultType => typeof(T);

    /// <summary>
    /// Declares dependencies programmatically at runtime.
    /// Override this method to add dynamic or conditional dependencies.
    /// </summary>
    /// <param name="deps">The dependency declaration builder.</param>
    /// <remarks>
    /// <para>
    /// Dependencies declared here are combined with attribute-based dependencies
    /// from <see cref="Attributes.DependsOnAttribute"/>.
    /// </para>
    /// <para>
    /// This method is called once during module initialization, before execution begins.
    /// </para>
    /// </remarks>
    /// <example>
    /// <code>
    /// protected override void DeclareDependencies(IDependencyDeclaration deps)
    /// {
    ///     deps.DependsOn&lt;RequiredModule&gt;();
    ///     deps.DependsOnOptional&lt;OptionalModule&gt;();
    ///     deps.DependsOnIf&lt;ConditionalModule&gt;(SomeCondition);
    ///     deps.DependsOnLazy&lt;HeavyModule&gt;();
    /// }
    /// </code>
    /// </example>
    protected virtual void DeclareDependencies(IDependencyDeclaration deps)
    {
        // Default implementation does nothing - dependencies are declared via attributes only
    }

    /// <summary>
    /// Internal method to collect declared dependencies.
    /// </summary>
    internal IReadOnlyList<DeclaredDependency> GetDeclaredDependencies()
    {
        var declaration = new DependencyDeclaration();
        DeclareDependencies(declaration);
        return declaration.Dependencies;
    }

    /// <summary>
    /// Executes the module's core logic.
    /// </summary>
    /// <param name="context">The module context providing access to pipeline services.</param>
    /// <param name="cancellationToken">A token that will be cancelled if the pipeline fails or the module times out.</param>
    /// <returns>The result of the module execution, or null.</returns>
    public abstract Task<T?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken);

    /// <summary>
    /// Called before the module executes. Override to add setup logic.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    protected virtual Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <summary>
    /// Called after the module completes (success or failure). Override to add cleanup or result transformation.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="result">The module result.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A modified result, or null to keep the original.</returns>
    protected virtual Task<ModuleResult<T>?> OnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
        => Task.FromResult<ModuleResult<T>?>(null);

    /// <summary>
    /// Called when the module is skipped. Override to add skip notification logic.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="skipDecision">The skip decision with reason.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    protected virtual Task OnSkippedAsync(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <summary>
    /// Called when the module fails with an exception. Override to add error handling.
    /// Called before OnAfterExecuteAsync.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A task representing the operation.</returns>
    protected virtual Task OnFailedAsync(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
        => Task.CompletedTask;

    /// <summary>
    /// Internal accessor to invoke OnBeforeExecuteAsync from the engine.
    /// </summary>
    internal Task InvokeOnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
        => OnBeforeExecuteAsync(context, cancellationToken);

    /// <summary>
    /// Internal accessor to invoke OnAfterExecuteAsync from the engine.
    /// </summary>
    internal Task<ModuleResult<T>?> InvokeOnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
        => OnAfterExecuteAsync(context, result, cancellationToken);

    /// <summary>
    /// Internal accessor to invoke OnSkippedAsync from the engine.
    /// </summary>
    internal Task InvokeOnSkippedAsync(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
        => OnSkippedAsync(context, skipDecision, cancellationToken);

    /// <summary>
    /// Internal accessor to invoke OnFailedAsync from the engine.
    /// </summary>
    internal Task InvokeOnFailedAsync(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
        => OnFailedAsync(context, exception, cancellationToken);

    /// <summary>
    /// Gets an awaiter for this module's result.
    /// </summary>
    public TaskAwaiter<ModuleResult<T?>> GetAwaiter() => CompletionSource.Task.GetAwaiter();
}
