using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "component", "continues-export", "update")]
public record AzMonitorAppInsightsComponentContinuesExportUpdateOptions(
[property: CommandSwitch("--id")] string Id
) : AzOptions
{
    [CommandSwitch("--app")]
    public string? App { get; set; }

    [CommandSwitch("--dest-account")]
    public int? DestAccount { get; set; }

    [CommandSwitch("--dest-container")]
    public string? DestContainer { get; set; }

    [CommandSwitch("--dest-sas")]
    public string? DestSas { get; set; }

    [CommandSwitch("--dest-sub-id")]
    public string? DestSubId { get; set; }

    [CommandSwitch("--dest-type")]
    public string? DestType { get; set; }

    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [BooleanCommandSwitch("--is-enabled")]
    public bool? IsEnabled { get; set; }

    [CommandSwitch("--record-types")]
    public string? RecordTypes { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}