using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blueprint", "resource-group", "add")]
public record AzBlueprintResourceGroupAddOptions(
[property: CommandSwitch("--blueprint-name")] string BlueprintName
) : AzOptions
{
    [CommandSwitch("--artifact-name")]
    public string? ArtifactName { get; set; }

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
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

