using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// A condition that returns true when running in a CI environment.
/// </summary>
/// <remarks>
/// Uses <see cref="IBuildSystemContext.IsBuildServer"/>: a known build system is detected, or the
/// <c>CI</c> environment variable is set to a value other than <c>false</c> or <c>0</c>.
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
        return Task.FromResult(context.Environment.BuildSystem.IsBuildServer);
    }
}
