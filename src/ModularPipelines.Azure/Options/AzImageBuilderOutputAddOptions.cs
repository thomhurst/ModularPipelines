using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("image", "builder", "output", "add")]
public record AzImageBuilderOutputAddOptions : AzOptions
{
    [CliOption("--artifact-tags")]
    public string? ArtifactTags { get; set; }

    [CliOption("--defer")]
    public string? Defer { get; set; }

    [CliOption("--gallery-image-definition")]
    public string? GalleryImageDefinition { get; set; }

    [CliOption("--gallery-name")]
    public string? GalleryName { get; set; }

    [CliOption("--gallery-replication-regions")]
    public string? GalleryReplicationRegions { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--is-vhd")]
    public string? IsVhd { get; set; }

    [CliOption("--managed-image")]
    public string? ManagedImage { get; set; }

    [CliOption("--managed-image-location")]
    public string? ManagedImageLocation { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--output-name")]
    public string? OutputName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--versioning")]
    public string? Versioning { get; set; }

    [CliOption("--vhd-uri")]
    public string? VhdUri { get; set; }
}