using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedblockchain", "get-member")]
public record AwsManagedblockchainGetMemberOptions(
[property: CommandSwitch("--network-id")] string NetworkId,
[property: CommandSwitch("--member-id")] string MemberId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}