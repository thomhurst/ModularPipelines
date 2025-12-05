using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain", "update-member")]
public record AwsManagedblockchainUpdateMemberOptions(
[property: CliOption("--network-id")] string NetworkId,
[property: CliOption("--member-id")] string MemberId
) : AwsOptions
{
    [CliOption("--log-publishing-configuration")]
    public string? LogPublishingConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}