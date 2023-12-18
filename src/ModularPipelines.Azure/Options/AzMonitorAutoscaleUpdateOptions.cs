using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "autoscale", "update")]
public record AzMonitorAutoscaleUpdateOptions : AzOptions
{
    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--add-action")]
    public string? AddAction { get; set; }

    [CommandSwitch("--autoscale-name")]
    public string? AutoscaleName { get; set; }

    [CommandSwitch("--count")]
    public int? Count { get; set; }

    [BooleanCommandSwitch("--email-administrator")]
    public bool? EmailAdministrator { get; set; }

    [BooleanCommandSwitch("--email-coadministrators")]
    public bool? EmailCoadministrators { get; set; }

    [BooleanCommandSwitch("--enabled")]
    public bool? Enabled { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--max-count")]
    public int? MaxCount { get; set; }

    [CommandSwitch("--min-count")]
    public int? MinCount { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--remove-action")]
    public string? RemoveAction { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--scale-look-ahead-time")]
    public string? ScaleLookAheadTime { get; set; }

    [CommandSwitch("--scale-mode")]
    public string? ScaleMode { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--tags")]
    public string? Tags { get; set; }
}