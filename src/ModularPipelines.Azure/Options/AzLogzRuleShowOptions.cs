using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("logz", "rule", "show")]
public record AzLogzRuleShowOptions : AzOptions
{
    [CliOption("--ids")]
    public string? Ids { get; set; }

    [CliOption("--monitor-name")]
    public string? MonitorName { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--rule-set-name")]
    public string? RuleSetName { get; set; }

    [CliOption("--subscription")]
    public new string? Subscription { get; set; }
}