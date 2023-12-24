using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedblockchain-query", "list-transactions")]
public record AwsManagedblockchainQueryListTransactionsOptions(
[property: CommandSwitch("--address")] string Address,
[property: CommandSwitch("--network")] string Network
) : AwsOptions
{
    [CommandSwitch("--from-blockchain-instant")]
    public string? FromBlockchainInstant { get; set; }

    [CommandSwitch("--to-blockchain-instant")]
    public string? ToBlockchainInstant { get; set; }

    [CommandSwitch("--sort")]
    public string? Sort { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--page-size")]
    public int? PageSize { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}