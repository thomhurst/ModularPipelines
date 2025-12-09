using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blockchain", "transaction-node", "regenerate-api-key")]
public record AzBlockchainTransactionNodeRegenerateApiKeyOptions(
[property: CliOption("--member-name")] string MemberName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CliOption("--key-name")]
    public string? KeyName { get; set; }

    [CliOption("--value")]
    public string? Value { get; set; }
}