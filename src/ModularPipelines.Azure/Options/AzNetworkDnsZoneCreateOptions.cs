using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "dns", "zone", "create")]
public record AzNetworkDnsZoneCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--if-none-match")]
    public bool? IfNoneMatch { get; set; }

    [CliOption("--parent-name")]
    public string? ParentName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}