using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medical-imaging", "update-image-set-metadata")]
public record AwsMedicalImagingUpdateImageSetMetadataOptions(
[property: CliOption("--datastore-id")] string DatastoreId,
[property: CliOption("--image-set-id")] string ImageSetId,
[property: CliOption("--latest-version-id")] string LatestVersionId,
[property: CliOption("--update-image-set-metadata-updates")] string UpdateImageSetMetadataUpdates
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}