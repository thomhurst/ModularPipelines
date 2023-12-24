using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedblockchain", "get-proposal")]
public record AwsManagedblockchainGetProposalOptions(
[property: CommandSwitch("--network-id")] string NetworkId,
[property: CommandSwitch("--proposal-id")] string ProposalId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}