using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("vmware", "placement-policy", "vm-host", "create")]
public record AzVmwarePlacementPolicyVmHostCreateOptions(
[property: CommandSwitch("--affinity-type")] string AffinityType,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--host-members")] string HostMembers,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--private-cloud")] string PrivateCloud,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--vm-members")] string VmMembers
) : AzOptions
{
    [CommandSwitch("--affinity-strength")]
    public string? AffinityStrength { get; set; }

    [CommandSwitch("--azure-hybrid-benefit")]
    public string? AzureHybridBenefit { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--state")]
    public string? State { get; set; }
}

