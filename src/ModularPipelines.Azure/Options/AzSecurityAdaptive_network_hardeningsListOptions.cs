using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("security", "adaptive_network_hardenings", "list")]
public record AzSecurityAdaptive_network_hardeningsListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--resource-name")] string ResourceName,
[property: CliOption("--resource-namespace")] string ResourceNamespace,
[property: CliOption("--resource-type")] string ResourceType
) : AzOptions;