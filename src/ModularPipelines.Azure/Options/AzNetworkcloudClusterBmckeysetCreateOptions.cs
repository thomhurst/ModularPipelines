using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkcloud", "cluster", "bmckeyset", "create")]
public record AzNetworkcloudClusterBmckeysetCreateOptions(
[property: CliOption("--azure-group-id")] string AzureGroupId,
[property: CliOption("--bmc-key-set-name")] string BmcKeySetName,
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--expiration")] string Expiration,
[property: CliOption("--extended-location")] string ExtendedLocation,
[property: CliOption("--privilege-level")] string PrivilegeLevel,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--user-list")] string UserList
) : AzOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}