using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedblockchain-query", "get-token-balance")]
public record AwsManagedblockchainQueryGetTokenBalanceOptions(
[property: CommandSwitch("--token-identifier")] string TokenIdentifier,
[property: CommandSwitch("--owner-identifier")] string OwnerIdentifier
) : AwsOptions
{
    [CommandSwitch("--at-blockchain-instant")]
    public string? AtBlockchainInstant { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}