using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("managedblockchain", "get-node")]
public record AwsManagedblockchainGetNodeOptions(
[property: CliOption("--network-id")] string NetworkId,
[property: CliOption("--node-id")] string NodeId
) : AwsOptions
{
    [CliOption("--member-id")]
    public string? MemberId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}