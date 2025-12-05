using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "health-checks", "list")]
public record GcloudComputeHealthChecksListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--regexp")]
    public string? Regexp { get; set; }

    [CliFlag("--global")]
    public bool? Global { get; set; }

    [CliOption("--regions")]
    public string[]? Regions { get; set; }
}