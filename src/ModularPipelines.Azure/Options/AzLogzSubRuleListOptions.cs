using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("logz", "sub-rule", "list")]
public record AzLogzSubRuleListOptions(
[property: CliOption("--monitor-name")] string MonitorName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--sub-account-name")] int SubAccountName
) : AzOptions;