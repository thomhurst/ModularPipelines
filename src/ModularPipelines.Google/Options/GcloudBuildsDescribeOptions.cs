using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("builds", "describe")]
public record GcloudBuildsDescribeOptions(
[property: CliArgument] string Build
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}