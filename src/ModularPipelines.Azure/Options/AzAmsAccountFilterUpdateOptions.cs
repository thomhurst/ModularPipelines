using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "account-filter", "update")]
public record AzAmsAccountFilterUpdateOptions : AzOptions
{
    [CommandSwitch("--account-name")]
    public int? AccountName { get; set; }

    [CommandSwitch("--add")]
    public string? Add { get; set; }

    [CommandSwitch("--end-timestamp")]
    public string? EndTimestamp { get; set; }

    [CommandSwitch("--first-quality")]
    public string? FirstQuality { get; set; }

    [BooleanCommandSwitch("--force-end-timestamp")]
    public bool? ForceEndTimestamp { get; set; }

    [BooleanCommandSwitch("--force-string")]
    public bool? ForceString { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--live-backoff-duration")]
    public string? LiveBackoffDuration { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--presentation-window-duration")]
    public string? PresentationWindowDuration { get; set; }

    [CommandSwitch("--remove")]
    public string? Remove { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--set")]
    public string? Set { get; set; }

    [CommandSwitch("--start-timestamp")]
    public string? StartTimestamp { get; set; }

    [CommandSwitch("--subscription")]
    public string? Subscription { get; set; }

    [CommandSwitch("--timescale")]
    public string? Timescale { get; set; }

    [CommandSwitch("--tracks")]
    public string? Tracks { get; set; }
}