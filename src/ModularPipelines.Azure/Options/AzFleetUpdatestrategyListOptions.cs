using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fleet", "updatestrategy", "list")]
public record AzFleetUpdatestrategyListOptions(
[property: CommandSwitch("--fleet-name")] string FleetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;