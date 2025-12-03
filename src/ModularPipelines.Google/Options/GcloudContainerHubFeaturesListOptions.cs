using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("container", "hub", "features", "list")]
public record GcloudContainerHubFeaturesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}