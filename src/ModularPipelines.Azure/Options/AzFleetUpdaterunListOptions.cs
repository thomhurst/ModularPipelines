using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fleet", "updaterun", "list")]
public record AzFleetUpdaterunListOptions(
[property: CommandSwitch("--fleet-name")] string FleetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;