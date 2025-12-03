using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("spanner", "instances", "create")]
public record GcloudSpannerInstancesCreateOptions(
[property: CliArgument] string Instance,
[property: CliOption("--config")] string Config,
[property: CliOption("--description")] string Description
) : GcloudOptions
{
    [CliFlag("--async")]
    public bool? Async { get; set; }

    [CliOption("--expire-behavior")]
    public string? ExpireBehavior { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--nodes")]
    public string? Nodes { get; set; }

    [CliOption("--processing-units")]
    public string? ProcessingUnits { get; set; }
}