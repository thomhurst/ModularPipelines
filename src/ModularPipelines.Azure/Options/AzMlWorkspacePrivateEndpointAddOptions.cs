using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ml", "workspace", "private-endpoint", "add")]
public record AzMlWorkspacePrivateEndpointAddOptions : AzOptions
{
    [CommandSwitch("--output-metadata-file")]
    public string? OutputMetadataFile { get; set; }

    [CommandSwitch("--path")]
    public string? Path { get; set; }

    [BooleanCommandSwitch("--pe-auto-approval")]
    public bool? PeAutoApproval { get; set; }

    [CommandSwitch("--pe-location")]
    public string? PeLocation { get; set; }

    [CommandSwitch("--pe-name")]
    public string? PeName { get; set; }

    [CommandSwitch("--pe-resource-group")]
    public string? PeResourceGroup { get; set; }

    [CommandSwitch("--pe-subnet-name")]
    public string? PeSubnetName { get; set; }

    [CommandSwitch("--pe-subscription-id")]
    public string? PeSubscriptionId { get; set; }

    [CommandSwitch("--pe-vnet-name")]
    public string? PeVnetName { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription-id")]
    public string? SubscriptionId { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }

    [CommandSwitch("--workspace-name")]
    public string? WorkspaceName { get; set; }
}