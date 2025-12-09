using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "enable-image-deprecation")]
public record AwsEc2EnableImageDeprecationOptions(
[property: CliOption("--image-id")] string ImageId,
[property: CliOption("--deprecate-at")] long DeprecateAt
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}