using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "app-insights", "component", "continues-export", "create")]
public record AzMonitorAppInsightsComponentContinuesExportCreateOptions(
[property: CliOption("--app")] string App,
[property: CliOption("--dest-account")] int DestAccount,
[property: CliOption("--dest-container")] string DestContainer,
[property: CliOption("--dest-sas")] string DestSas,
[property: CliOption("--dest-sub-id")] string DestSubId,
[property: CliOption("--record-types")] string RecordTypes,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--dest-type")]
    public string? DestType { get; set; }

    [CliFlag("--is-enabled")]
    public bool? IsEnabled { get; set; }
}