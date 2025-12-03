using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("connectedmachine", "run-command", "create")]
public record AzConnectedmachineRunCommandCreateOptions(
[property: CliOption("--machine-name")] string MachineName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--command-id")]
    public string? CommandId { get; set; }

    [CliOption("--error-blob-managed-identity")]
    public string? ErrorBlobManagedIdentity { get; set; }

    [CliOption("--error-blob-uri")]
    public string? ErrorBlobUri { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

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

    [CliOption("--run-as-user")]
    public string? RunAsUser { get; set; }

    [CliOption("--script")]
    public string? Script { get; set; }

    [CliOption("--script-uri")]
    public string? ScriptUri { get; set; }

    [CliOption("--script-uri-managed-id")]
    public string? ScriptUriManagedId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--timeout")]
    public string? Timeout { get; set; }
}