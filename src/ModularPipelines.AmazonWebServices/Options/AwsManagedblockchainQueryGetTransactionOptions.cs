using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain-query", "get-transaction")]
public record AwsManagedblockchainQueryGetTransactionOptions(
[property: CliOption("--transaction-hash")] string TransactionHash,
[property: CliOption("--network")] string Network
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}