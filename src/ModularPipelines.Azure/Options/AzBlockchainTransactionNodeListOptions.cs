using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blockchain", "transaction-node", "list")]
public record AzBlockchainTransactionNodeListOptions(
[property: CommandSwitch("--member-name")] string MemberName,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions;