using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("afd", "rule", "action", "list")]
public record AzAfdRuleActionListOptions(
[property: CliOption("--profile-name")] string ProfileName,
[property: CliOption("--resource-group")] string ResourceGroup,
[property: CliOption("--rule-name")] string RuleName,
[property: CliOption("--rule-set-name")] string RuleSetName
) : AzOptions;