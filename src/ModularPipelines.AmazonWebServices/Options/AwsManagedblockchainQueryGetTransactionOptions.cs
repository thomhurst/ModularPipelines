using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedblockchain-query", "get-transaction")]
public record AwsManagedblockchainQueryGetTransactionOptions(
[property: CommandSwitch("--transaction-hash")] string TransactionHash,
[property: CommandSwitch("--network")] string Network
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}