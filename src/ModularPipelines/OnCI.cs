using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// A condition that returns true when running in a CI environment.
/// </summary>
/// <remarks>
/// Checks for the presence of the <c>CI</c> environment variable, which is
/// set by most CI providers (GitHub Actions, Azure Pipelines, GitLab CI, etc.).
/// </remarks>
/// <example>
/// <code>
/// [RunIf&lt;OnCI&gt;]
/// public class PublishModule : Module&lt;None&gt;
/// {
///     // Only runs in CI, skipped locally
/// }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public sealed class OnCI : IPlanningRunCondition
{
    /// <inheritdoc />
    public Task<bool> EvaluateAsync(IPipelineContext context)
    {
        var ciEnvVar = context.Environment.Variables.Get("CI");
        var isCI = !string.IsNullOrEmpty(ciEnvVar) &&
                   !string.Equals(ciEnvVar, "false", StringComparison.OrdinalIgnoreCase);
        return Task.FromResult(isCI);
    }
}
