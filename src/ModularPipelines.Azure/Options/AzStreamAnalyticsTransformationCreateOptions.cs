using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stream-analytics", "transformation", "create")]
public record AzStreamAnalyticsTransformationCreateOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--if-none-match")]
    public string? IfNoneMatch { get; set; }

    [CommandSwitch("--saql")]
    public string? Saql { get; set; }

    [CommandSwitch("--streaming-units")]
    public string? StreamingUnits { get; set; }

    [CommandSwitch("--valid-streaming-units")]
    public string? ValidStreamingUnits { get; set; }
}

