using ModularPipelines.Context;

namespace ModularPipelines.Conditions;

/// <summary>
/// Base class for creating reusable groups of conditions.
/// </summary>
/// <remarks>
/// <para>
/// Condition groups allow you to define a set of conditions once and reuse them
/// across multiple modules. The group specifies how its conditions are combined
/// (AND or OR logic).
/// </para>
/// <para>
/// <b>Example - Unix platforms group:</b>
/// <code>
/// public sealed class OnUnixPlatforms : ConditionGroup
/// {
///     public override IRunCondition[] Conditions =&gt; [new OnLinux(), new OnMacOS()];
///     public override ConditionLogic Logic =&gt; ConditionLogic.Any;
/// }
///
/// // Usage:
/// [RunIfAny&lt;OnUnixPlatforms&gt;]
/// public class UnixModule : Module&lt;None&gt; { }
/// </code>
/// </para>
/// </remarks>
public abstract class ConditionGroup : IRunCondition
{
    /// <summary>
    /// Gets the conditions in this group.
    /// </summary>
    public abstract IRunCondition[] Conditions { get; }

    /// <summary>
    /// Gets the logic used to combine conditions in this group.
    /// </summary>
    public abstract ConditionLogic Logic { get; }

    /// <summary>
    /// Evaluates all conditions in the group according to the specified logic.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>
    /// For <see cref="ConditionLogic.All"/>: <c>true</c> if all conditions pass.
    /// For <see cref="ConditionLogic.Any"/>: <c>true</c> if any condition passes.
    /// </returns>
    public Task<bool> EvaluateAsync(IPipelineHookContext context) =>
        EvaluateAsync(context, CancellationToken.None);

    /// <inheritdoc />
    public Task<bool> EvaluateAsync(IPipelineHookContext context, CancellationToken cancellationToken)
    {
        if (Conditions.Length == 0)
        {
            return Task.FromResult(true);
        }

        return Logic switch
        {
            ConditionLogic.All => RunConditionEvaluator.EvaluateAllAsync(Conditions, context, cancellationToken),
            ConditionLogic.Any => RunConditionEvaluator.EvaluateAnyAsync(Conditions, context, cancellationToken),
            ConditionLogic.Skip => RunConditionEvaluator.EvaluateAnyAsync(Conditions, context, cancellationToken),
            _ => throw new ArgumentOutOfRangeException(nameof(Logic), Logic, "Unknown condition logic"),
        };
    }
}
