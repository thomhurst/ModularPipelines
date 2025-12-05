using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("vmware", "workload-network", "dhcp", "server", "create")]
public record AzVmwareWorkloadNetworkDhcpServerCreateOptions(
[property: CliOption("--dhcp")] string Dhcp,
[property: CliOption("--private-cloud")] string PrivateCloud,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--lease-time")]
    public string? LeaseTime { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--revision")]
    public string? Revision { get; set; }

    [CliOption("--server-address")]
    public string? ServerAddress { get; set; }
}