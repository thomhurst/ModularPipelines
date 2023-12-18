using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("monitor", "data-collection", "rule", "syslog", "list")]
public record AzMonitorDataCollectionRuleSyslogListOptions(
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-name")] string RuleName
) : AzOptions;