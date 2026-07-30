using Microsoft.Extensions.Logging;

namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Formatter for Azure Pipelines build system.
/// Supports collapsible log groups and secret variable registration via logging commands.
/// </summary>
/// <remarks>
/// Azure Pipelines uses logging commands in the format ##[command]value
/// Documentation: https://learn.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands.
/// </remarks>
/// <example>
/// <code>
/// // Example output:
/// // ##[group]Build Step
/// // ... log content ...
/// // ##[endgroup]
/// //
/// // ##vso[task.setvariable variable=mySecret;issecret=true]password123
/// </code>
/// </example>
internal class AzurePipelinesFormatter : IBuildSystemFormatter
{
    public bool UsesRawCommands => true;

    public string GetStartBlockCommand(string name) => $"##[group]{name}";

    public string GetEndBlockCommand(string name) => "##[endgroup]";

    public string GetMaskSecretCommand(string secret) =>
        $"##vso[task.setvariable variable=secret_{Guid.NewGuid():N};issecret=true]{EscapeLoggingCommandValue(secret)}";

    public string? GetLogIssueCommand(LogLevel logLevel, string message)
    {
        var issueType = logLevel switch
        {
            LogLevel.Critical or LogLevel.Error => "error",
            LogLevel.Warning => "warning",
            _ => null,
        };

        return issueType is null
            ? null
            : $"##vso[task.logissue type={issueType};]{EscapeLoggingCommandValue(message)}";
    }

    private static string EscapeLoggingCommandValue(string value)
    {
        return value
            .Replace("%", "%AZP25", StringComparison.Ordinal)
            .Replace(";", "%3B", StringComparison.Ordinal)
            .Replace("\r", "%0D", StringComparison.Ordinal)
            .Replace("\n", "%0A", StringComparison.Ordinal)
            .Replace("]", "%5D", StringComparison.Ordinal);
    }
}
