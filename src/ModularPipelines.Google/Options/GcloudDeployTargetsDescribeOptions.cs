using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("deploy", "targets", "describe")]
public record GcloudDeployTargetsDescribeOptions(
[property: PositionalArgument] string Target,
[property: PositionalArgument] string Region
) : GcloudOptions
{
    [CommandSwitch("--delivery-pipeline")]
    public string? DeliveryPipeline { get; set; }

    [BooleanCommandSwitch("--list-all-pipelines")]
    public bool? ListAllPipelines { get; set; }

    [BooleanCommandSwitch("--skip-pipeline-lookup")]
    public bool? SkipPipelineLookup { get; set; }
}