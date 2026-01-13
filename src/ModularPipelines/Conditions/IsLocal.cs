using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines.Conditions;

/// <summary>
/// A condition that returns true when running locally (not in CI).
/// </summary>
/// <remarks>
/// Returns true when the <c>CI</c> environment variable is not set or is "false".
/// </remarks>
/// <example>
/// <code>
/// [RunIfAll&lt;IsLocal&gt;]
/// public class LocalDevModule : Module&lt;None&gt;
/// {
///     // Only runs locally, skipped in CI
/// }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public sealed class IsLocal : IRunCondition
{
    /// <inheritdoc />
    public Task<bool> EvaluateAsync(IPipelineHookContext context)
    {
        var ciEnvVar = ((IPipelineEnvironment)context).Environment.EnvironmentVariables.GetEnvironmentVariable("CI");
        var isLocal = string.IsNullOrEmpty(ciEnvVar) ||
                      string.Equals(ciEnvVar, "false", StringComparison.OrdinalIgnoreCase);
        return Task.FromResult(isLocal);
    }
}
