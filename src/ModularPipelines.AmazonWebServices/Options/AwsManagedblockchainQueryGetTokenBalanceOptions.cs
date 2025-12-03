using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain-query", "get-token-balance")]
public record AwsManagedblockchainQueryGetTokenBalanceOptions(
[property: CliOption("--token-identifier")] string TokenIdentifier,
[property: CliOption("--owner-identifier")] string OwnerIdentifier
) : AwsOptions
{
    [CliOption("--at-blockchain-instant")]
    public string? AtBlockchainInstant { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}