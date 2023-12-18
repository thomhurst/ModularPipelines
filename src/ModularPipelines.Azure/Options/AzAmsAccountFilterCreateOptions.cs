using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ams", "account-filter", "create")]
public record AzAmsAccountFilterCreateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--end-timestamp")]
    public string? EndTimestamp { get; set; }

    [CommandSwitch("--first-quality")]
    public string? FirstQuality { get; set; }

    [BooleanCommandSwitch("--force-end-timestamp")]
    public bool? ForceEndTimestamp { get; set; }

    [CommandSwitch("--live-backoff-duration")]
    public string? LiveBackoffDuration { get; set; }

    [CommandSwitch("--presentation-window-duration")]
    public string? PresentationWindowDuration { get; set; }

    [CommandSwitch("--start-timestamp")]
    public string? StartTimestamp { get; set; }

    [CommandSwitch("--timescale")]
    public string? Timescale { get; set; }

    [CommandSwitch("--tracks")]
    public string? Tracks { get; set; }
}