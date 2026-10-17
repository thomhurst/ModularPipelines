using ModularPipelines.Context;

namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Formatter for TeamCity build system.
/// Supports collapsible log blocks and password masking via service messages.
/// </summary>
/// <remarks>
/// TeamCity uses service messages in the format ##teamcity[command name='value']
/// Documentation: https://www.jetbrains.com/help/teamcity/service-messages.html.
/// </remarks>
/// <example>
/// <code>
/// // Example output:
/// // ##teamcity[blockOpened name='Build Step']
/// // ... log content ...
/// // ##teamcity[blockClosed name='Build Step']
/// //
/// // ##teamcity[setParameter name='system.password.my-secret' value='password123']
/// </code>
/// </example>
internal class TeamCityFormatter : IBuildSystemFormatter
{
    public bool UsesRawCommands => true;

    public string GetStartBlockCommand(string name) => $"##teamcity[blockOpened name='{EscapeValue(name)}']";

    public string GetEndBlockCommand(string name) => $"##teamcity[blockClosed name='{EscapeValue(name)}']";

    public string GetMaskSecretCommand(string secret) => $"##teamcity[setParameter name='system.password.{Guid.NewGuid()}' value='{EscapeValue(secret)}']";

    public string GetOutputCommand(string name, string value) =>
        $"##teamcity[setParameter name='{EscapeValue(name)}' value='{EscapeValue(value)}']";

    public string GetBuildNumberCommand(string buildNumber) => $"##teamcity[buildNumber '{EscapeValue(buildNumber)}']";

    public string GetAnnotationCommand(BuildAnnotationLevel level, string message, string? file, int? line)
    {
        var status = level switch
        {
            BuildAnnotationLevel.Error => "ERROR",
            BuildAnnotationLevel.Warning => "WARNING",
            _ => "NORMAL",
        };
        if (file is not null)
        {
            var location = line is null ? file : $"{file}({line.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)})";
            message = $"{location}: {message}";
        }

        return $"##teamcity[message text='{EscapeValue(message)}' status='{status}']";
    }

    private static string EscapeValue(string value)
    {
        return value
            .Replace("|", "||")
            .Replace("'", "|'")
            .Replace("\n", "|n")
            .Replace("\r", "|r")
            .Replace("[", "|[")
            .Replace("]", "|]");
    }
}
