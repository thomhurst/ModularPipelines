using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Context;

namespace ModularPipelines;

/// <summary>
/// A condition that returns true when running locally (not in CI).
/// </summary>
/// <remarks>
/// The inverse of <see cref="OnCI"/>: no known build system is detected and the <c>CI</c> environment
/// variable is unset, <c>false</c> or <c>0</c>. See <see cref="IBuildSystemContext.IsBuildServer"/>.
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
        return Task.FromResult(!context.Environment.BuildSystem.IsBuildServer);
    }
}
