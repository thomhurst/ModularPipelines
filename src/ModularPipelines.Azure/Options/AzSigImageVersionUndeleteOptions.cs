using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sig", "image-version", "undelete")]
public record AzSigImageVersionUndeleteOptions(
[property: CliOption("--gallery-image-definition")] string GalleryImageDefinition,
[property: CliOption("--gallery-image-version")] string GalleryImageVersion,
[property: CliOption("--gallery-name")] string GalleryName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliFlag("--allow-replicated-location-deletion")]
    public bool? AllowReplicatedLocationDeletion { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}