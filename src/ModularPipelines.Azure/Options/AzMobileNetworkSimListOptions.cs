using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mobile-network", "sim", "list")]
public record AzMobileNetworkSimListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sim-group-name")] string SimGroupName
) : AzOptions;