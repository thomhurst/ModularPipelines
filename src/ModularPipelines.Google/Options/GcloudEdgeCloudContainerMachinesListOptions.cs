using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("edge-cloud", "container", "machines", "list")]
public record GcloudEdgeCloudContainerMachinesListOptions : GcloudOptions
{
    [CliOption("--location")]
    public string? Location { get; set; }
}