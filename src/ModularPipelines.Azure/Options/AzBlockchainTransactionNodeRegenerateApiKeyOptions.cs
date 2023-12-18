using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Context;
using ModularPipelines.Models;
using ModularPipelines.Options;

namespace ModularPipelines.Azure.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("blockchain", "transaction-node", "regenerate-api-key")]
public record AzBlockchainTransactionNodeRegenerateApiKeyOptions(
[property: CommandSwitch("--member-name")] string MemberName,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--resource-group")] string ResourceGroup
) : AzOptions
{
    [CommandSwitch("--key-name")]
    public string? KeyName { get; set; }

    [CommandSwitch("--value")]
    public string? Value { get; set; }
}