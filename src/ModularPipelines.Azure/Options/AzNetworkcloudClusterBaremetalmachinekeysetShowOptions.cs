using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "cluster", "baremetalmachinekeyset", "show")]
public record AzNetworkcloudClusterBaremetalmachinekeysetShowOptions : AzOptions
{
    [CliOption("--bare-metal-machine-key-set-name")]
    public string? BareMetalMachineKeySetName { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}