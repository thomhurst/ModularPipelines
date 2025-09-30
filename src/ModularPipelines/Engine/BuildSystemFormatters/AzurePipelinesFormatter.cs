namespace ModularPipelines.Engine.BuildSystemFormatters;

/// <summary>
/// Formatter for Azure Pipelines build system.
/// Supports collapsible log groups and secret variable registration via logging commands.
/// </summary>
/// <remarks>
/// Azure Pipelines uses logging commands in the format ##[command]value
/// Documentation: https://learn.microsoft.com/en-us/azure/devops/pipelines/scripts/logging-commands
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
    public string GetStartBlockCommand(string name) => $"##[group]{name}";

    public string GetEndBlockCommand(string name) => "##[endgroup]";

    public string GetMaskSecretCommand(string secret) => $"##vso[task.setvariable variable=secret_{Guid.NewGuid():N};issecret=true]{secret}";
}