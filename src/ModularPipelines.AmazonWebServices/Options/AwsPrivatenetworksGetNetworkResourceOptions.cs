using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("privatenetworks", "get-network-resource")]
public record AwsPrivatenetworksGetNetworkResourceOptions(
[property: CliOption("--network-resource-arn")] string NetworkResourceArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}