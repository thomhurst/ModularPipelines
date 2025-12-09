using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("networkcloud", "cluster", "baremetalmachinekeyset", "create")]
public record AzNetworkcloudClusterBaremetalmachinekeysetCreateOptions(
[property: CliOption("--azure-group-id")] string AzureGroupId,
[property: CliOption("--bare-metal-machine-key-set-name")] string BareMetalMachineKeySetName,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--expiration")] string Expiration,
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--jump-hosts-allowed")] string JumpHostsAllowed,
[property: CliOption("--privilege-level")] string PrivilegeLevel,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--user-list")] string UserList
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--os-group-name")]
    public string? OsGroupName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}