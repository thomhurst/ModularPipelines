using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "workspace", "private-endpoint", "delete")]
public record AzMlWorkspacePrivateEndpointDeleteOptions : AzOptions
{
    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--pe-connection-name")]
    public string? PeConnectionName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}