namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Formatter for TeamCity build system.
/// Supports collapsible log blocks and password masking via service messages.
/// </summary>
/// <remarks>
/// TeamCity uses service messages in the format ##teamcity[command name='value']
/// Documentation: https://www.jetbrains.com/help/teamcity/service-messages.html
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
    public string GetStartBlockCommand(string name) => $"##teamcity[blockOpened name='{EscapeValue(name)}']";

    public string GetEndBlockCommand(string name) => $"##teamcity[blockClosed name='{EscapeValue(name)}']";

    public string GetMaskSecretCommand(string secret) => $"##teamcity[setParameter name='system.password.{Guid.NewGuid()}' value='{EscapeValue(secret)}']";

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