using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("monitor", "app-insights", "component", "billing", "update")]
public record AzMonitorAppInsightsComponentBillingUpdateOptions : AzOptions
{
    [CliOption("--add")]
    public string? Add { get; set; }

    [CliOption("--app")]
    public string? App { get; set; }

    [CliOption("--cap")]
    public string? Cap { get; set; }

    [CliFlag("--force-string")]
    public bool? ForceString { get; set; }

    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--remove")]
    public string? Remove { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--set")]
    public string? Set { get; set; }

    [CliFlag("--stop")]
    public bool? Stop { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}