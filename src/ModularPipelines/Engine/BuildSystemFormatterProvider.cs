using ModularPipelines.Engine.BuildSystemFormatters;
using ModularPipelines.Enums;

namespace ModularPipelines.Engine;

/// <summary>
/// Provides the appropriate build system formatter based on the detected build system.
/// Acts as a factory for <see cref="IBuildSystemFormatter"/> implementations.
/// </summary>
/// <example>
/// <code>
/// var provider = new BuildSystemFormatterProvider(buildSystemDetector);
/// var formatter = provider.GetFormatter();
///
/// // Use formatter for current build system
/// var startCommand = formatter.GetStartBlockCommand("My Section");
/// if (startCommand != null)
/// {
///     console.WriteLine(startCommand);
/// }
/// </code>
/// </example>
internal class BuildSystemFormatterProvider : IBuildSystemFormatterProvider
{
    private readonly IBuildSystemDetector _buildSystemDetector;
    private readonly Dictionary<BuildSystem, IBuildSystemFormatter> _formatters;

    public BuildSystemFormatterProvider(IBuildSystemDetector buildSystemDetector)
    {
        _buildSystemDetector = buildSystemDetector;
        _formatters = new Dictionary<BuildSystem, IBuildSystemFormatter>
        {
            [BuildSystem.GitHubActions] = new GitHubActionsFormatter(),
            [BuildSystem.TeamCity] = new TeamCityFormatter(),
            [BuildSystem.AzurePipelines] = new AzurePipelinesFormatter(),
            [BuildSystem.GitLab] = new GitLabFormatter(),
            [BuildSystem.Jenkins] = new JenkinsFormatter(),
            [BuildSystem.Bitbucket] = new BitbucketFormatter(),
            [BuildSystem.TravisCI] = new TravisCIFormatter(),
            [BuildSystem.AppVeyor] = new AppVeyorFormatter(),
            [BuildSystem.Unknown] = new DefaultFormatter(),
        };
    }

    public IBuildSystemFormatter GetFormatter()
    {
        var buildSystem = _buildSystemDetector.GetCurrentBuildSystem();
        return _formatters.GetValueOrDefault(buildSystem) ?? new DefaultFormatter();
    }

    public IBuildSystemFormatter GetFormatter(BuildSystem buildSystem)
    {
        return _formatters.GetValueOrDefault(buildSystem) ?? new DefaultFormatter();
    }
}

/// <summary>
/// Interface for providing build system formatters.
/// </summary>
internal interface IBuildSystemFormatterProvider
{
    /// <summary>
    /// Gets the formatter for the currently detected build system.
    /// </summary>
    /// <returns>The appropriate build system formatter.</returns>
    IBuildSystemFormatter GetFormatter();

    /// <summary>
    /// Gets the formatter for a specific build system.
    /// </summary>
    /// <param name="buildSystem">The build system to get a formatter for.</param>
    /// <returns>The appropriate build system formatter.</returns>
    IBuildSystemFormatter GetFormatter(BuildSystem buildSystem);
}