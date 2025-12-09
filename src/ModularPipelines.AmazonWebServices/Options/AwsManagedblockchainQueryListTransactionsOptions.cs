using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain-query", "list-transactions")]
public record AwsManagedblockchainQueryListTransactionsOptions(
[property: CliOption("--address")] string Address,
[property: CliOption("--network")] string Network
) : AwsOptions
{
    [CliOption("--from-blockchain-instant")]
    public string? FromBlockchainInstant { get; set; }

    [CliOption("--to-blockchain-instant")]
    public string? ToBlockchainInstant { get; set; }

    [CliOption("--sort")]
    public string? Sort { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}