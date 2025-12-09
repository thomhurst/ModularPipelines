using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "manifest", "update-metadata")]
public record AzAcrManifestUpdateMetadataOptions : AzOptions
{
    [CliFlag("--delete-enabled")]
    public bool? DeleteEnabled { get; set; }

    [CliFlag("--list-enabled")]
    public bool? ListEnabled { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliFlag("--read-enabled")]
    public bool? ReadEnabled { get; set; }

    [CliOption("--registry")]
    public string? Registry { get; set; }

    [CliOption("--suffix")]
    public string? Suffix { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliFlag("--write-enabled")]
    public bool? WriteEnabled { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? MANIFESTID { get; set; }
}