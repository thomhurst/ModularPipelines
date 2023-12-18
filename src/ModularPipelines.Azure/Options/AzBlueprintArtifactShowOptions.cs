using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blueprint", "artifact", "show")]
public record AzBlueprintArtifactShowOptions(
[property: CommandSwitch("--blueprint-name")] string BlueprintName,
[property: CommandSwitch("--name")] string Name
) : AzOptions
{
    [CommandSwitch("--management-group")]
    public string? ManagementGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}