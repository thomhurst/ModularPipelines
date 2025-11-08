using ModularPipelines.Context;
using ModularPipelines.Models;

namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to provide custom skip logic for a module.
/// If this interface is not implemented, the module will never be skipped (unless configured via attributes).
/// </summary>
public interface IModuleSkipLogic
{
    /// <summary>
    /// Determines whether this module should be skipped.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <returns>A <see cref="SkipDecision"/> indicating whether to skip the module and why.</returns>
    Task<SkipDecision> ShouldSkipAsync(IPipelineContext context);
}
