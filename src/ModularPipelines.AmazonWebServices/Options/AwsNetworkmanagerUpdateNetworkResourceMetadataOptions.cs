using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("networkmanager", "update-network-resource-metadata")]
public record AwsNetworkmanagerUpdateNetworkResourceMetadataOptions(
[property: CliOption("--global-network-id")] string GlobalNetworkId,
[property: CliOption("--resource-arn")] string ResourceArn,
[property: CliOption("--metadata")] IEnumerable<KeyValue> Metadata
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}