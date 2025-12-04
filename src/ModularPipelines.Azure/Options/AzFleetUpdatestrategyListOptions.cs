using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("fleet", "updatestrategy", "list")]
public record AzFleetUpdatestrategyListOptions(
[property: CliOption("--fleet-name")] string FleetName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;