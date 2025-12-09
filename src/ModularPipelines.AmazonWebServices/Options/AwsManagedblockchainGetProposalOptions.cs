using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain", "get-proposal")]
public record AwsManagedblockchainGetProposalOptions(
[property: CliOption("--network-id")] string NetworkId,
[property: CliOption("--proposal-id")] string ProposalId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}