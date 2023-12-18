using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fleet", "member", "list")]
public record AzFleetMemberListOptions(
[property: CommandSwitch("--fleet-name")] string FleetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;