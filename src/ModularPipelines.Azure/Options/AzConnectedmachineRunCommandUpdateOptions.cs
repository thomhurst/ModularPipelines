using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedmachine", "run-command", "update")]
public record AzConnectedmachineRunCommandUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--error-blob-managed-identity")]
    public string? ErrorBlobManagedIdentity { get; set; }

    [CliOption("--error-blob-uri")]
    public string? ErrorBlobUri { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--machine-name")]
    public string? MachineName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--output-blob-managed-identity")]
    public string? OutputBlobManagedIdentity { get; set; }

    [CliOption("--output-blob-uri")]
    public string? OutputBlobUri { get; set; }

    [CliOption("--parameters")]
    public string[]? Parameters { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--protected-parameters")]
    public string? ProtectedParameters { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--run-as-user")]
    public string? RunAsUser { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--source")]
    public string? Source { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}