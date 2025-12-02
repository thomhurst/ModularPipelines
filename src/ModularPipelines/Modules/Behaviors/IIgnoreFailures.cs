using ModularPipelines.Context;

namespace ModularPipelines.Modules.Behaviors;

/// <summary>
/// Implement this interface to allow a module to fail without failing the entire pipeline.
/// </summary>
/// <remarks>
/// When a module's failure is ignored:
/// <list type="bullet">
/// <item>The module's status will be set to <see cref="Enums.Status.IgnoredFailure"/></item>
/// <item>The pipeline will continue executing other modules</item>
/// <item>Dependent modules will receive a result with the exception</item>
/// </list>
/// </remarks>
public interface IIgnoreFailures
{
    /// <summary>
    /// Determines whether a failure should be ignored for this module.
    /// </summary>
    /// <param name="context">The pipeline context.</param>
    /// <param name="exception">The exception that caused the failure.</param>
    /// <returns>True if the failure should be ignored; otherwise, false.</returns>
    Task<bool> ShouldIgnoreFailures(IPipelineContext context, Exception exception);
}
