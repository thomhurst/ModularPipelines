using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("acr", "manifest", "list")]
public record AzAcrManifestListOptions : AzOptions
{
    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--orderby")]
    public string? Orderby { get; set; }

    [CliOption("--password")]
    public string? Password { get; set; }

    [CliOption("--registry")]
    public string? Registry { get; set; }

    [CliOption("--suffix")]
    public string? Suffix { get; set; }

    [CliOption("--top")]
    public string? Top { get; set; }

    [CliOption("--username")]
    public string? Username { get; set; }

    [CliArgument(Placement = ArgumentPlacement.AfterOptions)]
    public string? REPOID { get; set; }
}