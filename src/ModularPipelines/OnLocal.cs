using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// A condition that returns true when running locally (not in CI).
/// </summary>
/// <remarks>
/// Returns true when the <c>CI</c> environment variable is not set or is "false".
/// </remarks>
/// <example>
/// <code>
/// [RunIf&lt;OnLocal&gt;]
/// public class LocalDevModule : Module&lt;None&gt;
/// {
///     // Only runs locally, skipped in CI
/// }
/// </code>
/// </example>
[ExcludeFromCodeCoverage]
public sealed class OnLocal : IPlanningRunCondition
{
    /// <inheritdoc />
    public Task<bool> EvaluateAsync(IPipelineContext context)
    {
        var ciEnvVar = context.Environment.Variables.Get("CI");
        var isLocal = string.IsNullOrEmpty(ciEnvVar) ||
                      string.Equals(ciEnvVar, "false", StringComparison.OrdinalIgnoreCase);
        return Task.FromResult(isLocal);
    }
}
