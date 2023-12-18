using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedmachine", "run-command", "create")]
public record AzConnectedmachineRunCommandCreateOptions(
[property: CommandSwitch("--machine-name")] string MachineName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--command-id")]
    public string? CommandId { get; set; }

    [CommandSwitch("--error-blob-managed-identity")]
    public string? ErrorBlobManagedIdentity { get; set; }

    [CommandSwitch("--error-blob-uri")]
    public string? ErrorBlobUri { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--output-blob-managed-identity")]
    public string? OutputBlobManagedIdentity { get; set; }

    [CommandSwitch("--output-blob-uri")]
    public string? OutputBlobUri { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--password")]
    public string? Password { get; set; }

    [CommandSwitch("--protected-parameters")]
    public string? ProtectedParameters { get; set; }

    [CommandSwitch("--run-as-user")]
    public string? RunAsUser { get; set; }

    [CommandSwitch("--script")]
    public string? Script { get; set; }

    [CommandSwitch("--script-uri")]
    public string? ScriptUri { get; set; }

    [CommandSwitch("--script-uri-managed-id")]
    public string? ScriptUriManagedId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}