using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("stream-analytics", "output", "update")]
public record AzStreamAnalyticsOutputUpdateOptions(
[property: CommandSwitch("--job-name")] string JobName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--datasource")]
    public string? Datasource { get; set; }

    [CommandSwitch("--if-match")]
    public string? IfMatch { get; set; }

    [CommandSwitch("--serialization")]
    public string? Serialization { get; set; }

    [CommandSwitch("--size-window")]
    public string? SizeWindow { get; set; }

    [CommandSwitch("--time-window")]
    public string? TimeWindow { get; set; }
}

