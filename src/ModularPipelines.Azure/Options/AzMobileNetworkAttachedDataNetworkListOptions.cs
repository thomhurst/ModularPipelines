using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("mobile-network", "attached-data-network", "list")]
public record AzMobileNetworkAttachedDataNetworkListOptions(
[property: CliOption("--pccp-name")] string PccpName,
[property: CliOption("--pcdp-name")] string PcdpName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;