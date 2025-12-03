using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("monitor", "app-insights", "component", "continues-export", "update")]
public record AzMonitorAppInsightsComponentContinuesExportUpdateOptions(
[property: CliOption("--id")] string Id
) : AzOptions
{
    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--dest-account")]
    public int? DestAccount { get; set; }

    [CliOption("--dest-container")]
    public string? DestContainer { get; set; }

    [CliOption("--dest-sas")]
    public string? DestSas { get; set; }

    [CliOption("--dest-sub-id")]
    public string? DestSubId { get; set; }

    [CliOption("--dest-type")]
    public string? DestType { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliFlag("--is-enabled")]
    public bool? IsEnabled { get; set; }

    [CliOption("--record-types")]
    public string? RecordTypes { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}