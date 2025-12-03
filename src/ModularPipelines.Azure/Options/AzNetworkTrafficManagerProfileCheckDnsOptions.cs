using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "traffic-manager", "profile", "check-dns")]
public record AzNetworkTrafficManagerProfileCheckDnsOptions : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }
}