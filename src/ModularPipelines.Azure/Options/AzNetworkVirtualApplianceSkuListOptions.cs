using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("network", "virtual-appliance", "sku", "list")]
public record AzNetworkVirtualApplianceSkuListOptions : AzOptions;