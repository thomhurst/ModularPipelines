using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CliSubCommand("blockchain", "transaction-node", "show")]
public record AzBlockchainTransactionNodeShowOptions(
[property: CliOption("--member-name")] string MemberName,
[property: CliOption("--name")] string Name,
[property: CliOption("--resource-group")] string ResourceGroup
) : AzOptions;