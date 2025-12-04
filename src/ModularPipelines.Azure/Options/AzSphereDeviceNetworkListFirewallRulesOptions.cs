using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sphere", "device", "network", "list-firewall-rules")]
public record AzSphereDeviceNetworkListFirewallRulesOptions : AzOptions
{
    [CliOption("--device")]
    public string? Device { get; set; }
}