using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blueprint", "artifact", "template", "create")]
public record AzBlueprintArtifactTemplateCreateOptions(
[property: CommandSwitch("--artifact-name")] string ArtifactName,
[property: CommandSwitch("--blueprint-name")] string BlueprintName,
[property: CommandSwitch("--template")] string Template
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
    public string? Subscription { get; set; }
}