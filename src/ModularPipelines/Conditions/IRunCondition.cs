using ModularPipelines.Context;

namespace ModularPipelines.Conditions;

/// <summary>
/// Defines a condition that determines whether a module should run.
/// </summary>
/// <remarks>
/// Implement this interface to create custom run conditions.
/// Conditions are evaluated before module execution and can access
/// the pipeline context for environment checks, HTTP calls, etc.
/// Conditions may also be evaluated while building a dry-run plan. Implementations must not
/// mutate state or rely on being evaluated exactly once.
/// </remarks>
/// <example>
/// <code>
/// public sealed class HasGitHubToken : IRunCondition
/// {
///     public Task&lt;bool&gt; EvaluateAsync(IPipelineContext context)
///         =&gt; Task.FromResult(!string.IsNullOrEmpty(
///             context.Environment.Variables.Get("GITHUB_TOKEN")));
/// }
/// </code>
/// </example>
public interface IRunCondition
{
    /// <summary>
    /// Evaluates the condition asynchronously.
    /// </summary>
    /// <param name="context">The pipeline context for accessing environment, HTTP, etc.</param>
    /// <returns>
    /// A task that returns <c>true</c> if the condition is satisfied; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> EvaluateAsync(IPipelineContext context);

    /// <summary>
    /// Evaluates the condition asynchronously with cancellation between composed conditions.
    /// </summary>
    /// <param name="context">The pipeline context for accessing environment, HTTP, etc.</param>
    /// <param name="cancellationToken">A token used to cancel condition evaluation.</param>
    /// <returns>A task that returns whether the condition is satisfied.</returns>
    Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return EvaluateAsync(context);
    }
}

internal static class RunConditionEvaluator
{
    public static async Task<bool> EvaluateAllAsync(
        IEnumerable<Func<IRunCondition>> conditionFactories,
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        foreach (var conditionFactory in conditionFactories)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!await conditionFactory().EvaluateAsync(context, cancellationToken).ConfigureAwait(false))
            {
                return false;
            }
        }

        return true;
    }

    public static async Task<bool> EvaluateAllAsync(
        IEnumerable<IRunCondition> conditions,
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        foreach (var condition in conditions)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (!await condition.EvaluateAsync(context, cancellationToken).ConfigureAwait(false))
            {
                return false;
            }
        }

        return true;
    }

    public static async Task<bool> EvaluateAnyAsync(
        IEnumerable<Func<IRunCondition>> conditionFactories,
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        foreach (var conditionFactory in conditionFactories)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (await conditionFactory().EvaluateAsync(context, cancellationToken).ConfigureAwait(false))
            {
                return true;
            }
        }

        return false;
    }

    public static async Task<bool> EvaluateAnyAsync(
        IEnumerable<IRunCondition> conditions,
        IPipelineContext context,
        CancellationToken cancellationToken)
    {
        foreach (var condition in conditions)
        {
            cancellationToken.ThrowIfCancellationRequested();
            if (await condition.EvaluateAsync(context, cancellationToken).ConfigureAwait(false))
            {
                return true;
            }
        }

        return false;
    }
}
