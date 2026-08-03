using ModularPipelines.Context;

namespace ModularPipelines.Modules;

#pragma warning disable SA1202 // Keep the user override before its protected-internal adapter.

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
/// All module features (dependencies, configuration, async lifecycle hooks, etc.) work
/// identically to <see cref="Module{T}"/>.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class VersionCalculator : SyncModule&lt;string&gt;
/// {
///     protected override string Execute(IModuleContext context, CancellationToken cancellationToken)
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
public abstract class SyncModule<T> : Module<T>
{
    /// <summary>
    /// Executes the module's core logic synchronously.
    /// </summary>
    /// <param name="context">The module context providing access to pipeline services.</param>
    /// <param name="cancellationToken">A token that will be cancelled if the pipeline fails or the module times out.</param>
    /// <returns>The result of the module execution.</returns>
    /// <remarks>
    /// <para>
    /// Implement this method to define your module's synchronous logic.
    /// </para>
    /// <para>
    /// <strong>Important:</strong> If your logic requires async operations (file I/O, HTTP calls, etc.),
    /// use <see cref="Module{T}"/> instead and implement <see cref="Module{T}.ExecuteAsync"/>.
    /// </para>
    /// </remarks>
    protected abstract T Execute(IModuleContext context, CancellationToken cancellationToken);

    /// <inheritdoc />
    /// <remarks>
    /// This method wraps the synchronous <see cref="Execute"/> method in a <see cref="Task"/>.
    /// You should not override this method in <see cref="SyncModule{T}"/> - override
    /// <see cref="Execute"/> instead.
    /// </remarks>
    protected internal sealed override Task<T> ExecuteAsync(
        IModuleContext context,
        CancellationToken cancellationToken) =>
        Task.FromResult(Execute(context, cancellationToken));
}

#pragma warning restore SA1202
