using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "describe-image-version")]
public record AwsSagemakerDescribeImageVersionOptions(
[property: CliOption("--image-name")] string ImageName
) : AwsOptions
{
    [CliOption("--alias")]
    public string? Alias { get; set; }

    [CliOption("--version-number")]
    public int? VersionNumber { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}