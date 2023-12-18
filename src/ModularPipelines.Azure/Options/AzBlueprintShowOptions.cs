using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blueprint", "show")]
public record AzBlueprintShowOptions(
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--management-group")]
    public string? ManagementGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}