using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain-query", "batch-get-token-balance")]
public record AwsManagedblockchainQueryBatchGetTokenBalanceOptions : AwsOptions
{
    [CliOption("--get-token-balance-inputs")]
    public string[]? GetTokenBalanceInputs { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}