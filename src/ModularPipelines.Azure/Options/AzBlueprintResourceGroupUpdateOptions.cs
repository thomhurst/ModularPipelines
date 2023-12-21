using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blueprint", "resource-group", "update")]
public record AzBlueprintResourceGroupUpdateOptions(
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

    [CommandSwitch("--rg-location")]
    public string? RgLocation { get; set; }

    [CommandSwitch("--rg-name")]
    public string? RgName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}