using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("networkcloud", "cluster", "baremetalmachinekeyset", "create")]
public record AzNetworkcloudClusterBaremetalmachinekeysetCreateOptions(
[property: CommandSwitch("--azure-group-id")] string AzureGroupId,
[property: CommandSwitch("--bare-metal-machine-key-set-name")] string BareMetalMachineKeySetName,
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--expiration")] string Expiration,
[property: CommandSwitch("--extended-location")] string ExtendedLocation,
[property: CommandSwitch("--jump-hosts-allowed")] string JumpHostsAllowed,
[property: CommandSwitch("--privilege-level")] string PrivilegeLevel,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--user-list")] string UserList
) : AzOptions
{
    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--os-group-name")]
    public string? OsGroupName { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

