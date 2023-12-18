using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("image", "builder", "output", "add")]
public record AzImageBuilderOutputAddOptions(
[property: CommandSwitch("--output-name")] string OutputName
) : AzOptions
{
    [CommandSwitch("--artifact-tags")]
    public string? ArtifactTags { get; set; }

    [CommandSwitch("--defer")]
    public string? Defer { get; set; }

    [CommandSwitch("--gallery-image-definition")]
    public string? GalleryImageDefinition { get; set; }

    [CommandSwitch("--gallery-name")]
    public string? GalleryName { get; set; }

    [CommandSwitch("--gallery-replication-regions")]
    public string? GalleryReplicationRegions { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--is-vhd")]
    public string? IsVhd { get; set; }

    [CommandSwitch("--managed-image")]
    public string? ManagedImage { get; set; }

    [CommandSwitch("--managed-image-location")]
    public string? ManagedImageLocation { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--versioning")]
    public string? Versioning { get; set; }

    [CommandSwitch("--vhd-uri")]
    public string? VhdUri { get; set; }
}