using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "cluster", "baremetalmachinekeyset", "update")]
public record AzNetworkcloudClusterBaremetalmachinekeysetUpdateOptions : AzOptions
{
    [CliOption("--bare-metal-machine-key-set-name")]
    public string? BareMetalMachineKeySetName { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--expiration")]
    public string? Expiration { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--jump-hosts-allowed")]
    public string? JumpHostsAllowed { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-list")]
    public string? UserList { get; set; }
}