using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "placement-policy", "vm-host", "update")]
public record AzVmwarePlacementPolicyVmHostUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--affinity-strength")]
    public string? AffinityStrength { get; set; }

    [CliOption("--azure-hybrid-benefit")]
    public string? AzureHybridBenefit { get; set; }

    [CliOption("--cluster-name")]
    public string? ClusterName { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--host-members")]
    public string? HostMembers { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--private-cloud")]
    public string? PrivateCloud { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--vm-members")]
    public string? VmMembers { get; set; }
}