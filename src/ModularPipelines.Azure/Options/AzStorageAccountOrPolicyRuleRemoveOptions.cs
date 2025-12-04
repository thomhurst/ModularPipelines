using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "or-policy", "rule", "remove")]
public record AzStorageAccountOrPolicyRuleRemoveOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--policy-id")] string PolicyId,
[property: CliOption("--rule-id")] string RuleId
) : AzOptions
{
    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}