using ModularPipelines.Enums;

namespace ModularPipelines.Context;

/// <summary>
/// Describes the CI/CD build system the pipeline is running on.
/// </summary>
/// <remarks>
/// <para>
/// A known build system is detected from the environment variables it sets (for example
/// <c>GITHUB_ACTIONS</c> or <c>TF_BUILD</c>). The pipeline is considered to run on a build server
/// when a known build system is detected, or otherwise when the <c>CI</c> environment variable is set
/// to a value other than <c>false</c> or <c>0</c>.
/// </para>
/// <para>
/// This is the single CI definition used by <see cref="OnCI"/>, <see cref="OnLocal"/>,
/// <c>Require.Ci</c>, and <c>Require.LocalEnvironment</c>.
/// </para>
/// </remarks>
public interface IBuildSystemContext
{
    /// <summary>
    /// Gets the detected build system, or <see cref="BuildSystem.Unknown"/> when none is recognised.
    /// </summary>
    BuildSystem Current { get; }

    /// <summary>
    /// Gets a value indicating whether the pipeline is running on a CI/CD build server.
    /// </summary>
    /// <remarks>
    /// <see langword="true"/> when <see cref="Current"/> is a known build system, or when the <c>CI</c>
    /// environment variable is set to a value other than <c>false</c> or <c>0</c>.
    /// </remarks>
    bool IsCI { get; }

    /// <summary>
    /// Gets a value indicating whether the pipeline is running locally rather than in CI.
    /// </summary>
    bool IsLocal => !IsCI;

    /// <summary>
    /// Gets a value indicating whether the pipeline is running on the specified build system.
    /// </summary>
    /// <param name="buildSystem">The build system to check for.</param>
    /// <returns><see langword="true"/> when <see cref="Current"/> equals <paramref name="buildSystem"/>.</returns>
    bool Is(BuildSystem buildSystem) => Current == buildSystem;
}
