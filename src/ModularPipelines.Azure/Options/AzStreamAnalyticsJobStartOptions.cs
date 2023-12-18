using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stream-analytics", "job", "start")]
public record AzStreamAnalyticsJobStartOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [BooleanCommandSwitch("--no-wait")]
    public bool? NoWait { get; set; }

    [CommandSwitch("--output-start-mode")]
    public string? OutputStartMode { get; set; }

    [CommandSwitch("--output-start-time")]
    public string? OutputStartTime { get; set; }
}

