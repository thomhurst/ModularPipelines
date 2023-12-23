using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedblockchain", "vote-on-proposal")]
public record AwsManagedblockchainVoteOnProposalOptions(
[property: CommandSwitch("--network-id")] string NetworkId,
[property: CommandSwitch("--proposal-id")] string ProposalId,
[property: CommandSwitch("--voter-member-id")] string VoterMemberId,
[property: CommandSwitch("--vote")] string Vote
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}