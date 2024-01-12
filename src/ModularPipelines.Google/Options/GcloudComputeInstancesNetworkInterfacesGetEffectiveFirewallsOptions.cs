using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("compute", "instances", "network-interfaces", "get-effective-firewalls")]
public record GcloudComputeInstancesNetworkInterfacesGetEffectiveFirewallsOptions(
[property: PositionalArgument] string InstanceName,
[property: PositionalArgument] string Name
) : GcloudOptions
{
    [CommandSwitch("--network-interface")]
    public string? NetworkInterface { get; set; }

    [CommandSwitch("--regexp")]
    public string? Regexp { get; set; }

    [CommandSwitch("--zone")]
    public string? Zone { get; set; }
}