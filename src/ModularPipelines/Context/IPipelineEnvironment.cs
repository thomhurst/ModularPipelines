using ModularPipelines.Engine;
using ModularPipelines.Helpers;

namespace ModularPipelines.Context;

/// <summary>
/// Provides access to environment and build system detection.
/// </summary>
/// <remarks>
/// <para>
/// This interface groups environment context and build system detection helpers
/// for better Interface Segregation.
/// </para>
/// <para><b>Thread Safety:</b></para>
/// <para>
/// All members of this interface are thread-safe for concurrent access.
/// Both <see cref="Environment"/> and <see cref="BuildSystemDetector"/> provide
/// read-only access to environment information that does not change during pipeline execution,
/// making them inherently safe for concurrent use from multiple threads.
/// </para>
/// </remarks>
public interface IPipelineEnvironment
{
    /// <summary>
    /// Gets helpers for getting information about the current environment.
    /// </summary>
    /// <remarks>
    /// Provides access to OS info, environment variables, working directory, etc.
    /// </remarks>
    IEnvironmentContext Environment { get; }

    /// <summary>
    /// Gets helpers for detecting the current build system.
    /// </summary>
    /// <remarks>
    /// Detects GitHub Actions, Azure DevOps, Jenkins, GitLab CI, etc.
    /// </remarks>
    IBuildSystemDetector BuildSystemDetector { get; }
}
