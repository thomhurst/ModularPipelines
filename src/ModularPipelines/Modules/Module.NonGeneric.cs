using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Modules;

/// <summary>
/// Base class for modules that don't return a meaningful result.
/// </summary>
/// <remarks>
/// <para>
/// Use this class when your module performs an action (like deploying, sending notifications,
/// or running tests) but doesn't need to pass data to dependent modules.
/// </para>
/// <para>
/// For modules that return data, use <see cref="Module{T}"/> instead.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public class DeployModule : Module
/// {
///     protected override async Task ExecuteModuleAsync(IModuleContext context, CancellationToken cancellationToken)
///     {
///         await context.Command.ExecuteCommandLineTool(...);
///         // No return statement needed
///     }
/// }
/// </code>
/// </example>
public abstract class Module : Module<None>
{
    /// <summary>
    /// Executes the module's core logic.
    /// </summary>
    /// <param name="context">The module context providing access to pipeline services.</param>
    /// <param name="cancellationToken">A token that will be cancelled if the pipeline fails or the module times out.</param>
    /// <returns>A task representing the asynchronous operation.</returns>
    /// <remarks>
    /// This method is called by the pipeline engine. Do not call it directly.
    /// Unlike <see cref="Module{T}.ExecuteAsync"/>, this method does not return a value.
    /// </remarks>
    protected abstract Task ExecuteModuleAsync(IModuleContext context, CancellationToken cancellationToken);

    /// <inheritdoc />
    protected internal sealed override async Task<None> ExecuteAsync(IModuleContext context, CancellationToken cancellationToken)
    {
        await ExecuteModuleAsync(context, cancellationToken);
        return None.Value;
    }
}
