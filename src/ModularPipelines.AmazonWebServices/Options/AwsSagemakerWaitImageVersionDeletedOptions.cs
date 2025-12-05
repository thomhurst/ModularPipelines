using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("sagemaker", "wait", "image-version-deleted")]
public record AwsSagemakerWaitImageVersionDeletedOptions(
[property: CliOption("--image-name")] string ImageName
) : AwsOptions
{
    [CliOption("--version")]
    public new int? Version { get; set; }

    [CliOption("--alias")]
    public string? Alias { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}