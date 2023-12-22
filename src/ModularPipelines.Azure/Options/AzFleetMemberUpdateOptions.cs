using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fleet", "member", "update")]
public record AzFleetMemberUpdateOptions(
[property: CommandSwitch("--fleet-name")] string FleetName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--update-group")]
    public string? UpdateGroup { get; set; }
}