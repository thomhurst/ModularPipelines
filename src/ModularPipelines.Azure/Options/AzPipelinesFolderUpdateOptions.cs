using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("pipelines", "folder", "update")]
public record AzPipelinesFolderUpdateOptions(
[property: CommandSwitch("--path")] string Path
) : AzOptions
{
    [BooleanCommandSwitch("--detect")]
    public bool? Detect { get; set; }

    [CommandSwitch("--new-description")]
    public string? NewDescription { get; set; }

    [CommandSwitch("--new-path")]
    public string? NewPath { get; set; }

    [CommandSwitch("--org")]
    public string? Org { get; set; }

    [CommandSwitch("--project")]
    public string? Project { get; set; }
}

