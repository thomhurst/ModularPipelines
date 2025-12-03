using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("new-relic", "monitor", "create")]
public record AzNewRelicMonitorCreateOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--account-creation-source")]
    public int? AccountCreationSource { get; set; }

    [CliOption("--identity")]
    public string? Identity { get; set; }

    [CliOption("--location")]
    public string? Location { get; set; }

    [CliOption("--new-relic-account")]
    public int? NewRelicAccount { get; set; }

    [CliFlag("--no-wait")]
    public bool? NoWait { get; set; }

    [CliOption("--org-creation-source")]
    public string? OrgCreationSource { get; set; }

    [CliOption("--plan-data")]
    public string? PlanData { get; set; }

    [CliOption("--tags")]
    public string? Tags { get; set; }

    [CliOption("--user-info")]
    public string? UserInfo { get; set; }
}