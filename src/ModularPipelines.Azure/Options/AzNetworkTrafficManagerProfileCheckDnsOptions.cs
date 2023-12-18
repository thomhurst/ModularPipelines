using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("network", "traffic-manager", "profile", "check-dns")]
public record AzNetworkTrafficManagerProfileCheckDnsOptions : AzOptions
{
    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--type")]
    public string? Type { get; set; }
}