using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disks", "list")]
public record GcloudComputeDisksListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--regexp")]
    public string? Regexp { get; set; }

    [CliOption("--regions")]
    public string[]? Regions { get; set; }

    [CliOption("--zones")]
    public string[]? Zones { get; set; }
}