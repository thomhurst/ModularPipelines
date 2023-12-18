using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "traffic-manager", "profile", "check-dns")]
public record AzNetworkTrafficManagerProfileCheckDnsOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--routing-method")] string RoutingMethod,
[property: CommandSwitch("--unique-dns-name")] string UniqueDnsName
) : AzOptions
{
    [CommandSwitch("--type")]
    public string? Type { get; set; }
}