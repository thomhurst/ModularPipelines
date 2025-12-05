using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "regions", "list")]
public record GcloudComputeRegionsListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--regexp")]
    public string? Regexp { get; set; }
}