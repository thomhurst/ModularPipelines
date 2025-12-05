using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("imagebuilder", "delete-image-pipeline")]
public record AwsImagebuilderDeleteImagePipelineOptions(
[property: CliOption("--image-pipeline-arn")] string ImagePipelineArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}