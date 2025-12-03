using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "network-interfaces", "get-effective-firewalls")]
public record GcloudComputeInstancesNetworkInterfacesGetEffectiveFirewallsOptions(
[property: CliArgument] string InstanceName,
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--network-interface")]
    public string? NetworkInterface { get; set; }

    [CliOption("--regexp")]
    public string? Regexp { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}