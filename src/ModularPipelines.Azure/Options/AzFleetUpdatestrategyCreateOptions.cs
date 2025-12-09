using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("fleet", "updatestrategy", "create")]
public record AzFleetUpdatestrategyCreateOptions(
[property: CliOption("--fleet-name")] string FleetName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--stages")] string Stages
) : AzOptions
{
    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }
}