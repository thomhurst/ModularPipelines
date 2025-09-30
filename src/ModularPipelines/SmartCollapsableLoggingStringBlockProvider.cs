using ModularPipelines.Engine;

namespace ModularPipelines;

/// <summary>
/// Provides build system-specific collapsible log group markers.
/// Delegates to the appropriate formatter based on the current build system.
/// </summary>
/// <example>
/// <code>
/// var provider = new SmartCollapsableLoggingStringBlockProvider(formatterProvider);
///
/// // Get start command for current build system
/// var startCommand = provider.GetStartConsoleLogGroup("Build Step");
/// // Returns "::group::Build Step" for GitHub Actions
/// // Returns "##teamcity[blockOpened name='Build Step']" for TeamCity
/// // Returns "\e[0Ksection_start:...:build_step\r\e[0KBuild Step" for GitLab
///
/// // Get end command
/// var endCommand = provider.GetEndConsoleLogGroup("Build Step");
/// </code>
/// </example>
internal class SmartCollapsableLoggingStringBlockProvider : ISmartCollapsableLoggingStringBlockProvider
{
    private readonly IBuildSystemFormatterProvider _formatterProvider;

    public SmartCollapsableLoggingStringBlockProvider(IBuildSystemFormatterProvider formatterProvider)
    {
        _formatterProvider = formatterProvider;
    }

    public string? GetStartConsoleLogGroup(string name)
    {
        var formatter = _formatterProvider.GetFormatter();
        return formatter.GetStartBlockCommand(name);
    }

    public string? GetEndConsoleLogGroup(string name)
    {
        var formatter = _formatterProvider.GetFormatter();
        return formatter.GetEndBlockCommand(name);
    }
}