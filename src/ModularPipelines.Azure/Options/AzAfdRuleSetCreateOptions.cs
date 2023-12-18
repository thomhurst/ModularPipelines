using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("afd", "rule-set", "create")]
public record AzAfdRuleSetCreateOptions(
[property: CommandSwitch("--profile-name")] string ProfileName,
[property: CommandSwitch("--resource-group")] string ResourceGroup,
[property: CommandSwitch("--rule-set-name")] string RuleSetName
) : AzOptions;