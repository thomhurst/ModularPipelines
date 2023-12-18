using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("sig", "image-definition", "list-shared")]
public record AzSigImageDefinitionListSharedOptions(
[property: CommandSwitch("--gallery-image-definition")] string GalleryImageDefinition,
[property: CommandSwitch("--gallery-name")] string GalleryName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--gallery-unique-name")]
    public string? GalleryUniqueName { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--location")]
    public string? Location { get; set; }

    [CommandSwitch("--marker")]
    public string? Marker { get; set; }

    [CommandSwitch("--shared-to")]
    public string? SharedTo { get; set; }

    [CommandSwitch("--show-next-marker")]
    public string? ShowNextMarker { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }
}

