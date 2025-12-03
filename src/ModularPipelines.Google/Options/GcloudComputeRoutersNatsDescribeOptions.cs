using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "nats", "describe")]
public record GcloudComputeRoutersNatsDescribeOptions(
[property: CliArgument] string Name,
[property: CliOption("--router")] string Router
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}