using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "routers", "nats", "delete")]
public record GcloudComputeRoutersNatsDeleteOptions(
[property: CliArgument] string Name,
[property: CliOption("--router")] string Router
) : GcloudOptions
{
    [CliOption("--region")]
    public string? Region { get; set; }
}