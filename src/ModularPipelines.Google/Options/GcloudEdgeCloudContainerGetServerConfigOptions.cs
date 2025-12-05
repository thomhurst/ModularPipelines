using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "get-server-config")]
public record GcloudEdgeCloudContainerGetServerConfigOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}