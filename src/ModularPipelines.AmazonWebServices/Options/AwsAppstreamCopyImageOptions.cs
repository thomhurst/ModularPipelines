using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "copy-image")]
public record AwsAppstreamCopyImageOptions(
[property: CliOption("--source-image-name")] string SourceImageName,
[property: CliOption("--destination-image-name")] string DestinationImageName,
[property: CliOption("--destination-region")] string DestinationRegion
) : AwsOptions
{
    [CliOption("--destination-image-description")]
    public string? DestinationImageDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}