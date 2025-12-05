using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("sentinel", "automation-rule", "list")]
public record AzSentinelAutomationRuleListOptions(
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--workspace-name")] string WorkspaceName
) : AzOptions;