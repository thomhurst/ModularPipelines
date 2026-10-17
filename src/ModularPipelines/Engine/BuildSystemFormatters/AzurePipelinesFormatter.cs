using Microsoft.Extensions.Logging;
using ModularPipelines.Context;

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

    public string GetOutputCommand(string name, string value) =>
        $"##vso[task.setvariable variable={EscapeLoggingCommandValue(name)};isOutput=true;]{EscapeLoggingCommandValue(value)}";

    public string GetBuildNumberCommand(string buildNumber) =>
        $"##vso[build.updatebuildnumber]{EscapeLoggingCommandValue(buildNumber)}";

    public string? GetAnnotationCommand(BuildAnnotationLevel level, string message, string? file, int? line)
    {
        if (level == BuildAnnotationLevel.Notice)
        {
            return null;
        }

        var type = level == BuildAnnotationLevel.Error ? "error" : "warning";
        var location = file is null ? string.Empty : $"sourcepath={EscapeLoggingCommandValue(file)};";
        if (line is not null)
        {
            location += $"linenumber={line.Value.ToString(System.Globalization.CultureInfo.InvariantCulture)};";
        }

        return $"##vso[task.logissue type={type};{location}]{EscapeLoggingCommandValue(message)}";
    }

    public string? GetLogIssueCommand(LogLevel logLevel, string message) =>
        logLevel switch
        {
            LogLevel.Critical or LogLevel.Error => GetAnnotationCommand(BuildAnnotationLevel.Error, message, null, null),
            LogLevel.Warning => GetAnnotationCommand(BuildAnnotationLevel.Warning, message, null, null),
            _ => null,
        };

    internal static string EscapeLoggingCommandValue(string value)
    {
        return value
            .Replace("%", "%AZP25", StringComparison.Ordinal)
            .Replace(";", "%3B", StringComparison.Ordinal)
            .Replace("\r", "%0D", StringComparison.Ordinal)
            .Replace("\n", "%0A", StringComparison.Ordinal)
            .Replace("]", "%5D", StringComparison.Ordinal);
    }
}
