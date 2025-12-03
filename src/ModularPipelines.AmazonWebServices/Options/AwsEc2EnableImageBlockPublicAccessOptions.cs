using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "enable-image-block-public-access")]
public record AwsEc2EnableImageBlockPublicAccessOptions(
[property: CliOption("--image-block-public-access-state")] string ImageBlockPublicAccessState
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}