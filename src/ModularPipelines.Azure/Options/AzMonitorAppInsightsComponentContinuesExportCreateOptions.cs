using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "app-insights", "component", "continues-export", "create")]
public record AzMonitorAppInsightsComponentContinuesExportCreateOptions(
[property: CommandSwitch("--app")] string App,
[property: CommandSwitch("--dest-account")] int DestAccount,
[property: CommandSwitch("--dest-container")] string DestContainer,
[property: CommandSwitch("--dest-sas")] string DestSas,
[property: CommandSwitch("--dest-sub-id")] string DestSubId,
[property: CommandSwitch("--record-types")] string RecordTypes,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--dest-type")]
    public string? DestType { get; set; }

    [BooleanCommandSwitch("--is-enabled")]
    public bool? IsEnabled { get; set; }
}