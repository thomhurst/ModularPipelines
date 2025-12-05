using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("compute", "instances", "get-screenshot")]
public record GcloudComputeInstancesGetScreenshotOptions(
[property: CliArgument] string InstanceName
) : GcloudOptions
{
    [CliOption("--destination")]
    public string? Destination { get; set; }

    [CliOption("--zone")]
    public string? Zone { get; set; }
}