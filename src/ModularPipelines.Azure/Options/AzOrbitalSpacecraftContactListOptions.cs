using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

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