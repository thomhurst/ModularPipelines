using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("network", "vhub", "routing-intent", "list")]
public record AzNetworkVhubRoutingIntentListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--vhub")] string Vhub
) : AzOptions;