using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("fleet", "member", "list")]
public record AzFleetMemberListOptions(
[property: CommandSwitch("--fleet-name")] string FleetName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--update-group")]
    public string? UpdateGroup { get; set; }
}

