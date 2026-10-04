using ModularPipelines.Enums;

namespace ModularPipelines.Context.Domains.Implementations;

/// <summary>
/// Adapter that wraps <see cref="IBuildSystemDetector"/> to provide the <see cref="IBuildSystemContext"/> interface.
/// </summary>
internal class BuildSystemContext(IBuildSystemDetector detector) : IBuildSystemContext
{
    /// <inheritdoc />
    public BuildSystem Current => detector.Current;

    /// <inheritdoc />
    public bool IsCI => detector.IsCI;

    /// <inheritdoc />
    public bool Is(BuildSystem buildSystem) => detector.Is(buildSystem);
}
