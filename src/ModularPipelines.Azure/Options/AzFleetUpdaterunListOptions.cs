using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("fleet", "updaterun", "list")]
public record AzFleetUpdaterunListOptions(
[property: CliOption("--fleet-name")] string FleetName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;