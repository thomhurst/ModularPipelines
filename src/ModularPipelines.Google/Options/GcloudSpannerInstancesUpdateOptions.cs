using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "instances", "update")]
public record GcloudSpannerInstancesUpdateOptions(
[property: CliArgument] string Instance
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--expire-behavior")]
    public string? ExpireBehavior { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--nodes")]
    public string? Nodes { get; set; }

    [CliOption("--processing-units")]
    public string? ProcessingUnits { get; set; }
}