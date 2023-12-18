using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "or-policy", "rule", "show")]
public record AzStorageAccountOrPolicyRuleShowOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--policy-id")] string PolicyId,
[property: CommandSwitch("--rule-id")] string RuleId
) : AzOptions
{
    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }
}