using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blueprint", "artifact", "policy", "update")]
public record AzBlueprintArtifactPolicyUpdateOptions(
[property: CommandSwitch("--artifact-name")] string ArtifactName,
[property: CommandSwitch("--blueprint-name")] string BlueprintName
) : AzOptions
{
    [CommandSwitch("--depends-on")]
    public string? DependsOn { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--management-group")]
    public string? ManagementGroup { get; set; }

    [CommandSwitch("--parameters")]
    public string[]? Parameters { get; set; }

    [CommandSwitch("--resource-group-art")]
    public string? ResourceGroupArt { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}