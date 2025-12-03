using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CliCommand("deploy", "targets", "describe")]
public record GcloudDeployTargetsDescribeOptions(
[property: CliArgument] string Target,
[property: CliArgument] string Region
) : GcloudOptions
{
    [CliOption("--delivery-pipeline")]
    public string? DeliveryPipeline { get; set; }

    [CliFlag("--list-all-pipelines")]
    public bool? ListAllPipelines { get; set; }

    [CliFlag("--skip-pipeline-lookup")]
    public bool? SkipPipelineLookup { get; set; }
}