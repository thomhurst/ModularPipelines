using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("acr", "manifest", "delete")]
public record AzAcrManifestDeleteOptions : AzOptions
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

    [CliFlag("--yes")]
    public bool? Yes { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? MANIFESTID { get; set; }
}