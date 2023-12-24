using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("managedblockchain", "update-member")]
public record AwsManagedblockchainUpdateMemberOptions(
[property: CommandSwitch("--network-id")] string NetworkId,
[property: CommandSwitch("--member-id")] string MemberId
) : AwsOptions
{
    [CommandSwitch("--log-publishing-configuration")]
    public string? LogPublishingConfiguration { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}