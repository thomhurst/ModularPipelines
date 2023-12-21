using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "data-collection", "rule", "syslog", "add")]
public record AzMonitorDataCollectionRuleSyslogAddOptions(
[property: CommandSwitch("--facility-names")] string FacilityNames,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--streams")] string Streams
) : AzOptions
{
    [CommandSwitch("--ids")]
    public string? Ids { get; set; }

    [CommandSwitch("--log-levels")]
    public string? LogLevels { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--rule-name")]
    public string? RuleName { get; set; }

    [CommandSwitch("--subscription")]
    public new string? Subscription { get; set; }
}