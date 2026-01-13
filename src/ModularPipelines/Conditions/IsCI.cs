using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Conditions;

/// <summary>
/// A condition that returns true when running in a CI environment.
/// </summary>
/// <remarks>
/// Checks for the presence of the <c>CI</c> environment variable, which is
/// set by most CI providers (GitHub Actions, Azure Pipelines, GitLab CI, etc.).
/// </remarks>
/// <example>
/// <code>
/// [RunIfAll&lt;IsCI&gt;]
/// public class PublishModule : Module&lt;None&gt;
/// {
///     // Only runs in CI, skipped locally
/// }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public sealed class IsCI : IRunCondition
{
    /// <inheritdoc />
    public Task<bool> EvaluateAsync(IPipelineHookContext context)
    {
        var ciEnvVar = ((IPipelineEnvironment)context).Environment.EnvironmentVariables.GetEnvironmentVariable("CI");
        var isCI = !string.IsNullOrEmpty(ciEnvVar) &&
                   !string.Equals(ciEnvVar, "false", StringComparison.OrdinalIgnoreCase);
        return Task.FromResult(isCI);
    }
}
