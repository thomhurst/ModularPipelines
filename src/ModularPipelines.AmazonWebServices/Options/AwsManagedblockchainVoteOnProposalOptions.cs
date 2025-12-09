using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain", "vote-on-proposal")]
public record AwsManagedblockchainVoteOnProposalOptions(
[property: CliOption("--network-id")] string NetworkId,
[property: CliOption("--proposal-id")] string ProposalId,
[property: CliOption("--voter-member-id")] string VoterMemberId,
[property: CliOption("--vote")] string Vote
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}