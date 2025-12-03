using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "virtual-appliance", "site", "create")]
public record AzNetworkVirtualApplianceSiteCreateOptions(
[property: CliOption("--appliance-name")] string ApplianceName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--address-prefix")]
    public string? AddressPrefix { get; set; }

    [CliFlag("--allow")]
    public bool? Allow { get; set; }

    [CliFlag("--default")]
    public bool? Default { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliFlag("--optimize")]
    public bool? Optimize { get; set; }
}