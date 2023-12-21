using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connectedmachine", "run-command", "update")]
public record AzConnectedmachineRunCommandUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--error-blob-managed-identity")]
    public string? ErrorBlobManagedIdentity { get; set; }

    [CommandSwitch("--error-blob-uri")]
    public string? ErrorBlobUri { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--machine-name")]
    public string? MachineName { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

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

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--run-as-user")]
    public string? RunAsUser { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--source")]
    public string? Source { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--timeout")]
    public string? Timeout { get; set; }
}