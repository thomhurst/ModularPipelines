using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Google.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("spanner", "samples", "run")]
public record GcloudSpannerSamplesRunOptions(
[property: PositionalArgument] string Appname,
[property: CommandSwitch("--instance-id")] string InstanceId
) : GcloudOptions
{
    [BooleanCommandSwitch("--cleanup")]
    public bool? Cleanup { get; set; }

    [CommandSwitch("--database-id")]
    public string? DatabaseId { get; set; }

    [CommandSwitch("--duration")]
    public string? Duration { get; set; }

    [BooleanCommandSwitch("--skip-init")]
    public bool? SkipInit { get; set; }
}