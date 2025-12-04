using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "or-policy", "rule", "update")]
public record AzStorageAccountOrPolicyRuleUpdateOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--policy-id")] string PolicyId,
[property: CliOption("--rule-id")] string RuleId
) : AzOptions
{
    [CliOption("--destination-container")]
    public string? DestinationContainer { get; set; }

    [CliOption("--min-creation-time")]
    public string? MinCreationTime { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }

    [CliOption("--source-container")]
    public string? SourceContainer { get; set; }
}