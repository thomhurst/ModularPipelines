using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliCommand("blockchain", "transaction-node", "list")]
public record AzBlockchainTransactionNodeListOptions(
[property: CliOption("--member-name")] string MemberName,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;