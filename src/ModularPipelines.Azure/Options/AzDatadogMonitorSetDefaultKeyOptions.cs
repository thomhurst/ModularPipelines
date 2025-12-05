using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datadog", "monitor", "set-default-key")]
public record AzDatadogMonitorSetDefaultKeyOptions : AzOptions
{
    [CliOption("--created")]
    public string? Created { get; set; }

    [CliOption("--created-by")]
    public string? CreatedBy { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--key")]
    public string? Key { get; set; }

    [CliOption("--monitor-name")]
    public string? MonitorName { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}