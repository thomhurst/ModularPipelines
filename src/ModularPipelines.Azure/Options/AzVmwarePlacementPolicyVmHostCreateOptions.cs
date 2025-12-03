using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("vmware", "placement-policy", "vm-host", "create")]
public record AzVmwarePlacementPolicyVmHostCreateOptions(
[property: CliOption("--affinity-type")] string AffinityType,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--host-members")] string HostMembers,
[property: CliOption("--name")] string Name,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vm-members")] string VmMembers
) : AzOptions
{
    [CliOption("--affinity-strength")]
    public string? AffinityStrength { get; set; }

    [CliOption("--azure-hybrid-benefit")]
    public string? AzureHybridBenefit { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--state")]
    public string? State { get; set; }
}