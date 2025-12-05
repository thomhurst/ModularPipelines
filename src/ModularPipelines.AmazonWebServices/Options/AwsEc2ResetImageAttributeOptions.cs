using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "reset-image-attribute")]
public record AwsEc2ResetImageAttributeOptions(
[property: CliOption("--attribute")] string Attribute,
[property: CliOption("--image-id")] string ImageId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}