using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("datadog", "monitor", "create")]
public record AzDatadogMonitorCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--datadog-organization-properties")]
    public string? DatadogOrganizationProperties { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--sku-name")]
    public string? SkuName { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--type")]
    public string? Type { get; set; }

    [CliOption("--user-info")]
    public string? UserInfo { get; set; }
}