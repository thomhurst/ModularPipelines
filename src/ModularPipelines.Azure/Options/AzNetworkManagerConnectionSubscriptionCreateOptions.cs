using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "manager", "connection", "subscription", "create")]
public record AzNetworkManagerConnectionSubscriptionCreateOptions(
[property: CliOption("--connection-name")] string ConnectionName,
[property: CliOption("--network-manager")] string NetworkManager
) : AzOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }
}