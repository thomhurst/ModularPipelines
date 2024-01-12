using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "instances", "update")]
public record GcloudSpannerInstancesUpdateOptions(
[property: PositionalArgument] string Instance
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--expire-behavior")]
    public string? ExpireBehavior { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--nodes")]
    public string? Nodes { get; set; }

    [CommandSwitch("--processing-units")]
    public string? ProcessingUnits { get; set; }
}