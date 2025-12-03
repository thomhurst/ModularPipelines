using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain", "delete-member")]
public record AwsManagedblockchainDeleteMemberOptions(
[property: CliOption("--network-id")] string NetworkId,
[property: CliOption("--member-id")] string MemberId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}