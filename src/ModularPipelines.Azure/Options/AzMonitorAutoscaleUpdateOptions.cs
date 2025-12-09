using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "autoscale", "update")]
public record AzMonitorAutoscaleUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--add-action")]
    public string? AddAction { get; set; }

    [CliOption("--autoscale-name")]
    public string? AutoscaleName { get; set; }

    [CliOption("--count")]
    public int? Count { get; set; }

    [CliFlag("--email-administrator")]
    public bool? EmailAdministrator { get; set; }

    [CliFlag("--email-coadministrators")]
    public bool? EmailCoadministrators { get; set; }

    [CliFlag("--enabled")]
    public bool? Enabled { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--max-count")]
    public int? MaxCount { get; set; }

    [CliOption("--min-count")]
    public int? MinCount { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--remove-action")]
    public string? RemoveAction { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--scale-look-ahead-time")]
    public string? ScaleLookAheadTime { get; set; }

    [CliOption("--scale-mode")]
    public string? ScaleMode { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }
}