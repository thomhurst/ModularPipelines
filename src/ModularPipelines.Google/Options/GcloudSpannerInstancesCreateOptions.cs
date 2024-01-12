using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "instances", "create")]
public record GcloudSpannerInstancesCreateOptions(
[property: PositionalArgument] string Instance,
[property: CommandSwitch("--config")] string Config,
[property: CommandSwitch("--description")] string Description
) : GcloudOptions
{
    [BooleanCommandSwitch("--async")]
    public bool? Async { get; set; }

    [CommandSwitch("--expire-behavior")]
    public string? ExpireBehavior { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

    [CommandSwitch("--nodes")]
    public string? Nodes { get; set; }

    [CommandSwitch("--processing-units")]
    public string? ProcessingUnits { get; set; }
}