using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "pcdp", "list")]
public record AzMobileNetworkPcdpListOptions(
[property: CliOption("--pccp-name")] string PccpName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;