using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("storage", "account", "or-policy", "rule", "update")]
public record AzStorageAccountOrPolicyRuleUpdateOptions(
[property: CommandSwitch("--account-name")] int AccountName,
[property: CommandSwitch("--policy-id")] string PolicyId,
[property: CommandSwitch("--rule-id")] string RuleId
) : AzOptions
{
    [CommandSwitch("--destination-container")]
    public string? DestinationContainer { get; set; }

    [CommandSwitch("--min-creation-time")]
    public string? MinCreationTime { get; set; }

    [CommandSwitch("--prefix")]
    public string? Prefix { get; set; }

    [CommandSwitch("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CommandSwitch("--source-container")]
    public string? SourceContainer { get; set; }
}