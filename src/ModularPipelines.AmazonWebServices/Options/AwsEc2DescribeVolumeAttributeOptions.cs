using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "describe-volume-attribute")]
public record AwsEc2DescribeVolumeAttributeOptions(
[property: CliOption("--attribute")] string Attribute,
[property: CliOption("--volume-id")] string VolumeId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}