using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("ml", "workspace", "private-endpoint", "add")]
public record AzMlWorkspacePrivateEndpointAddOptions : AzOptions
{
    [CliOption("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CliOption("--path")]
    public string? Path { get; set; }

    [CliFlag("--pe-auto-approval")]
    public bool? PeAutoApproval { get; set; }

    [CliOption("--pe-location")]
    public string? PeLocation { get; set; }

    [CliOption("--pe-name")]
    public string? PeName { get; set; }

    [CliOption("--pe-resource-group")]
    public string? PeResourceGroup { get; set; }

    [CliOption("--pe-subnet-name")]
    public string? PeSubnetName { get; set; }

    [CliOption("--pe-subscription-id")]
    public string? PeSubscriptionId { get; set; }

    [CliOption("--pe-vnet-name")]
    public string? PeVnetName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--workspace-name")]
    public string? WorkspaceName { get; set; }
}