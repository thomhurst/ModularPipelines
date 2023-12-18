using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("orbital", "spacecraft", "contact", "list")]
public record AzOrbitalSpacecraftContactListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--spacecraft-name")] string SpacecraftName
) : AzOptions
{
    [CommandSwitch("--skiptoken")]
    public string? Skiptoken { get; set; }
}

