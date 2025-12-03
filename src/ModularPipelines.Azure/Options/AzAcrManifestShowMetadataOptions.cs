using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "manifest", "show-metadata")]
public record AzAcrManifestShowMetadataOptions : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--registry")]
    public string? Registry { get; set; }

    [CliOption("--suffix")]
    public string? Suffix { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? MANIFESTID { get; set; }
}