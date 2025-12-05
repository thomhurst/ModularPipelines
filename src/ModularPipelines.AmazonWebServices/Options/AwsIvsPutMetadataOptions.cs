using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ivs", "put-metadata")]
public record AwsIvsPutMetadataOptions(
[property: CliOption("--channel-arn")] string ChannelArn,
[property: CliOption("--metadata")] string Metadata
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}