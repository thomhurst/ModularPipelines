using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("disk-pool", "list-outbound-network-dependency-endpoint")]
public record AzDiskPoolListOutboundNetworkDependencyEndpointOptions(
[property: CliOption("--disk-pool-name")] string DiskPoolName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;