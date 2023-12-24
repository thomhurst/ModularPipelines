using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedblockchain-query", "batch-get-token-balance")]
public record AwsManagedblockchainQueryBatchGetTokenBalanceOptions : AwsOptions
{
    [CommandSwitch("--get-token-balance-inputs")]
    public string[]? GetTokenBalanceInputs { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}