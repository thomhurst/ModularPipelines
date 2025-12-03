using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "virtual-appliance", "site", "list")]
public record AzNetworkVirtualApplianceSiteListOptions(
[property: CliOption("--appliance-name")] string ApplianceName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;