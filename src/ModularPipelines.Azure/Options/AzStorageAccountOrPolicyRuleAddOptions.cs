using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("storage", "account", "or-policy", "rule", "add")]
public record AzStorageAccountOrPolicyRuleAddOptions(
[property: CliOption("--account-name")] int AccountName,
[property: CliOption("--destination-container")] string DestinationContainer,
[property: CliOption("--policy-id")] string PolicyId,
[property: CliOption("--source-container")] string SourceContainer
) : AzOptions
{
    [CliOption("--min-creation-time")]
    public string? MinCreationTime { get; set; }

    [CliOption("--prefix")]
    public string? Prefix { get; set; }

    [CliOption("--resource-group")]
    public string? ResourceGroup { get; set; }
}