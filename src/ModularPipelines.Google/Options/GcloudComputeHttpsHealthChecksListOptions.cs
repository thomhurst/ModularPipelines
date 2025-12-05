using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "https-health-checks", "list")]
public record GcloudComputeHttpsHealthChecksListOptions(
[property: CliArgument] string Name
) : GcloudOptions
{
    [CliOption("--regexp")]
    public string? Regexp { get; set; }
}