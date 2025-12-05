using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "disk-types", "list")]
public record GcloudComputeDiskTypesListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--regexp")]
    public string? Regexp { get; set; }

    [CliOption("--zones")]
    public string[]? Zones { get; set; }
}