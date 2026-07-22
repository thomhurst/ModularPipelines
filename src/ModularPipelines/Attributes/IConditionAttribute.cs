using ModularPipelines.Conditions;
using ModularPipelines.Context;

namespace ModularPipelines.Attributes;

/// <summary>
/// Interface for condition attributes that can be evaluated at runtime.
/// </summary>
public interface IConditionAttribute
{
    /// <summary>
    /// Gets the logic used by this condition attribute.
    /// </summary>
    ConditionLogic Logic { get; }

    /// <summary>
    /// Evaluates the condition(s) in this attribute.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns><c>true</c> if the condition is satisfied; otherwise, <c>false</c>.</returns>
    Task<bool> EvaluateAsync(IPipelineHookContext context);

    /// <summary>
    /// Evaluates the condition(s) in this attribute with cancellation between conditions.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <param name="cancellationToken">A token used to cancel condition evaluation.</param>
    /// <returns><c>true</c> if the condition is satisfied; otherwise, <c>false</c>.</returns>
    Task<bool> EvaluateAsync(IPipelineHookContext context, CancellationToken cancellationToken)
    {
        cancellationToken.ThrowIfCancellationRequested();
        return EvaluateAsync(context);
    }

    /// <summary>
    /// Gets a human-readable string of the condition names for error messages.
    /// </summary>
    string ConditionNames { get; }
}
