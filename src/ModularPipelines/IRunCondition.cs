using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// Defines a predicate that decides whether a module should run.
/// </summary>
/// <remarks>
/// <para>
/// <see cref="IRunCondition"/> is the single predicate type for run conditions. Apply stateless
/// conditions with <see cref="RunIfAttribute{T}"/>, <see cref="RunIfAnyAttribute{T1,T2}"/>, or
/// <see cref="SkipIfAttribute{T}"/>, register them in <c>Configure</c> with
/// <see cref="ModuleConfigurationBuilder.WithRunIf{TCondition}"/> or
/// <see cref="ModuleConfigurationBuilder.WithSkipIf{TCondition}"/>, compose them with
/// <see cref="ConditionGroup"/>, or derive a stateful attribute from <see cref="RunConditionAttribute"/>.
/// </para>
/// <para>
/// Conditions may be evaluated while building a dry-run plan. Implementations must not mutate
/// state or rely on being evaluated exactly once. Implement <see cref="IPlanningSafe"/> as well when
/// the condition is also free of blocking work and remote I/O, so planning can resolve it.
/// </para>
/// </remarks>
/// <example>
/// <code>
/// public sealed class HasGitHubToken : IRunCondition, IPlanningSafe
/// {
///     public Task&lt;bool&gt; EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken)
///         =&gt; Task.FromResult(!string.IsNullOrEmpty(
///             context.Environment.Variables.Get("GITHUB_TOKEN")));
/// }
/// </code>
/// </example>
public interface IRunCondition
{
    /// <summary>
    /// Evaluates the condition.
    /// </summary>
    /// <param name="context">The pipeline context for accessing environment, HTTP, etc.</param>
    /// <param name="cancellationToken">A token that is cancelled when the pipeline stops evaluating conditions.</param>
    /// <returns>
    /// A task that returns <c>true</c> if the condition is satisfied; otherwise, <c>false</c>.
    /// </returns>
    Task<bool> EvaluateAsync(IPipelineContext context, CancellationToken cancellationToken);
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
