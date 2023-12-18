using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "manager", "connection", "subscription", "create")]
public record AzNetworkManagerConnectionSubscriptionCreateOptions(
[property: CommandSwitch("--connection-name")] string ConnectionName,
[property: CommandSwitch("--network-manager")] string NetworkManager
) : AzOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }
}