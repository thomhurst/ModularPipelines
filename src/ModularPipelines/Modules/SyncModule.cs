using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

/// <summary>
/// A synchronous version of <see cref="Module{T}"/> that provides a simpler programming model
/// when async operations are not needed.
/// </summary>
/// <typeparam name="T">The type of result returned by the module.</typeparam>
/// <remarks>
/// <para>
/// Use <see cref="SyncModule{T}"/> when your module logic is purely synchronous and you want
/// to avoid the overhead of async/await patterns. This is particularly useful for:
/// </para>
/// <list type="bullet">
/// <item><description>Modules that perform simple computations or data transformations</description></item>
/// <item><description>Modules that aggregate results from dependencies without making I/O calls</description></item>
/// <item><description>Modules that read from already-loaded configuration</description></item>
/// </list>
/// <para>
/// Internally, this class inherits from <see cref="Module{T}"/> and wraps the synchronous
/// <see cref="Execute"/> method in a <see cref="Task"/>, ensuring full compatibility with
/// the pipeline execution engine.
/// </para>
/// <para>
/// All module features (dependencies, configuration, hooks, etc.) work identically to
/// <see cref="Module{T}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class VersionCalculator : SyncModule&lt;string&gt;
/// {
///     protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
///     {
///         var major = Environment.GetEnvironmentVariable("MAJOR_VERSION") ?? "1";
///         var minor = Environment.GetEnvironmentVariable("MINOR_VERSION") ?? "0";
///         var patch = Environment.GetEnvironmentVariable("PATCH_VERSION") ?? "0";
///
///         return $"{major}.{minor}.{patch}";
///     }
/// }
/// </code>
/// </example>
/// <example>
/// <code>
/// [DependsOn&lt;BuildModule&gt;]
/// public class ArtifactPathResolver : SyncModule&lt;string&gt;
/// {
///     protected override string? Execute(IModuleContext context, CancellationToken cancellationToken)
///     {
///         var buildResult = context.GetModule&lt;BuildModule, BuildOutput&gt;();
///
///         // Synchronously access the result (it's already complete due to DependsOn)
///         return Path.Combine(buildResult.Value!.OutputPath, "artifacts");
///     }
/// }
/// </code>
/// </example>
public abstract class SyncModule<T> : Module<T>
{
    /// <summary>
    /// Executes the module's core logic synchronously.
    /// </summary>
    /// <param name="context">The module context providing access to pipeline services.</param>
    /// <param name="cancellationToken">A token that will be cancelled if the pipeline fails or the module times out.</param>
    /// <returns>The result of the module execution, or null.</returns>
    /// <remarks>
    /// <para>
    /// Implement this method to define your module's synchronous logic.
    /// </para>
    /// <para>
    /// <strong>Important:</strong> If your logic requires async operations (file I/O, HTTP calls, etc.),
    /// use <see cref="Module{T}"/> instead and implement <see cref="Module{T}.ExecuteAsync"/>.
    /// </para>
    /// </remarks>
    protected abstract T? Execute(IModuleContext context, CancellationToken cancellationToken);

    /// <inheritdoc />
    /// <remarks>
    /// This method wraps the synchronous <see cref="Execute"/> method in a <see cref="Task"/>.
    /// You should not override this method in <see cref="SyncModule{T}"/> - override
    /// <see cref="Execute"/> instead.
    /// </remarks>
    public sealed override Task<T?> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        var result = Execute(context, cancellationToken);
        return Task.FromResult(result);
    }

    /// <summary>
    /// Called before the module executes. Override to add synchronous setup logic.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>
    /// <para>
    /// This is a synchronous alternative to <see cref="Module{T}.OnBeforeExecuteAsync"/>.
    /// </para>
    /// <para>
    /// Exceptions thrown from this method will prevent <see cref="Execute"/> from running
    /// and will be propagated as a module failure.
    /// </para>
    /// </remarks>
    protected virtual void OnBeforeExecute(IModuleContext context, CancellationToken cancellationToken)
    {
    }

    /// <summary>
    /// Called after the module completes (success or failure). Override to add synchronous cleanup
    /// or result transformation.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="result">The module result (may contain an exception on failure).</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>A modified result, or null to keep the original.</returns>
    /// <remarks>
    /// <para>
    /// This is a synchronous alternative to <see cref="Module{T}.OnAfterExecuteAsync"/>.
    /// </para>
    /// <para>
    /// This hook is called after both successful execution and failures.
    /// Check <see cref="ModuleResult{T}.Exception"/> to determine if the module failed.
    /// </para>
    /// </remarks>
    protected virtual ModuleResult<T>? OnAfterExecute(
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
        => null;

    /// <summary>
    /// Called when the module is skipped. Override to add synchronous skip notification logic.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="skipDecision">The skip decision with reason.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    protected virtual void OnSkipped(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
    {
    }

    /// <summary>
    /// Called when the module fails with an exception. Override to add synchronous error handling.
    /// Called before OnAfterExecute.
    /// </summary>
    /// <param name="context">The module context.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <remarks>
    /// <para>
    /// This is a synchronous alternative to <see cref="Module{T}.OnFailedAsync"/>.
    /// </para>
    /// <para>
    /// This hook is called when <see cref="Execute"/> throws an exception.
    /// </para>
    /// </remarks>
    protected virtual void OnFailed(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
    }

    /// <inheritdoc />
    protected sealed override Task OnBeforeExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        OnBeforeExecute(context, cancellationToken);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected sealed override Task<ModuleResult<T>?> OnAfterExecuteAsync(
        IModuleContext context,
        ModuleResult<T> result,
        CancellationToken cancellationToken)
    {
        var modifiedResult = OnAfterExecute(context, result, cancellationToken);
        return Task.FromResult(modifiedResult);
    }

    /// <inheritdoc />
    protected sealed override Task OnSkippedAsync(
        IModuleContext context,
        SkipDecision skipDecision,
        CancellationToken cancellationToken)
    {
        OnSkipped(context, skipDecision, cancellationToken);
        return Task.CompletedTask;
    }

    /// <inheritdoc />
    protected sealed override Task OnFailedAsync(
        IModuleContext context,
        Exception exception,
        CancellationToken cancellationToken)
    {
        OnFailed(context, exception, cancellationToken);
        return Task.CompletedTask;
    }
}
