using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("orbital", "spacecraft", "create")]
public record AzOrbitalSpacecraftCreateOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--links")]
    public string? Links { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--norad-id")]
    public string? NoradId { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--title-line")]
    public string? TitleLine { get; set; }

    [CliOption("--tle-line1")]
    public string? TleLine1 { get; set; }

    [CliOption("--tle-line2")]
    public string? TleLine2 { get; set; }
}