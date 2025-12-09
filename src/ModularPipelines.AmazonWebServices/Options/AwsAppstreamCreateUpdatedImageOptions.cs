using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "create-updated-image")]
public record AwsAppstreamCreateUpdatedImageOptions(
[property: CliOption("--existing-image-name")] string ExistingImageName,
[property: CliOption("--new-image-name")] string NewImageName
) : AwsOptions
{
    [CliOption("--new-image-description")]
    public string? NewImageDescription { get; set; }

    [CliOption("--new-image-display-name")]
    public string? NewImageDisplayName { get; set; }

    [CliOption("--new-image-tags")]
    public IEnumerable<KeyValue>? NewImageTags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}