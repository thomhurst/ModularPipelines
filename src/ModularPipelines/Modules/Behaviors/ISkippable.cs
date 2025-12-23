using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to define conditions under which a module should be skipped.
/// </summary>
/// <remarks>
/// When a module is skipped:
/// <list type="bullet">
/// <item>Its <see cref="Module{T}.ExecuteAsync"/> method is not called</item>
/// <item>Dependent modules may still run (they receive a skipped result)</item>
/// <item>If a result repository is configured, historical results may be used</item>
/// </list>
/// </remarks>
public interface ISkippable
{
    /// <summary>
    /// Determines whether this module should be skipped.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>A <see cref="SkipDecision"/> indicating whether to skip and optionally a reason.</returns>
    Task<SkipDecision> ShouldSkip(IPipelineContext context);
}
