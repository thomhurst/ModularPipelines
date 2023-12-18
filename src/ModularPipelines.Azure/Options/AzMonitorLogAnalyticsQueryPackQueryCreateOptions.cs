using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "log-analytics", "query-pack", "query", "create")]
public record AzMonitorLogAnalyticsQueryPackQueryCreateOptions(
[property: CommandSwitch("--body")] string Body,
[property: CommandSwitch("--display-name")] string DisplayName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--query-pack-name")] string QueryPackName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--categories")]
    public string? Categories { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--resource-types")]
    public string? ResourceTypes { get; set; }

    [CommandSwitch("--solutions")]
    public string? Solutions { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}

