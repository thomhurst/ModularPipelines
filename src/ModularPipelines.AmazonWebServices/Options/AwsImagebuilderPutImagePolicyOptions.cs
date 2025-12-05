using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "put-image-policy")]
public record AwsImagebuilderPutImagePolicyOptions(
[property: CliOption("--image-arn")] string ImageArn,
[property: CliOption("--policy")] string Policy
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}