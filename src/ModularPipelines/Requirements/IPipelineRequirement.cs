using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Requirements;

/// <summary>
/// Defines a requirement that must be satisfied for pipeline execution.
/// </summary>
public interface IPipelineRequirement
{
    /// <summary>
    /// Evaluates whether the requirement is satisfied.
    /// </summary>
    /// <param name="context">The pipeline context for evaluation.</param>
    /// <returns>A task that represents the asynchronous requirement evaluation containing the decision.</returns>
    Task<RequirementDecision> MustAsync(IPipelineHookContext context);

    /// <summary>
    /// Gets the order in which this requirement should be evaluated relative to other requirements.
    /// Lower values are evaluated first.
    /// </summary>
    int Order => 0;
}
