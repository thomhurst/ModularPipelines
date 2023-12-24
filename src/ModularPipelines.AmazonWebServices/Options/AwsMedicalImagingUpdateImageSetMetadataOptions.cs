using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medical-imaging", "update-image-set-metadata")]
public record AwsMedicalImagingUpdateImageSetMetadataOptions(
[property: CommandSwitch("--datastore-id")] string DatastoreId,
[property: CommandSwitch("--image-set-id")] string ImageSetId,
[property: CommandSwitch("--latest-version-id")] string LatestVersionId,
[property: CommandSwitch("--update-image-set-metadata-updates")] string UpdateImageSetMetadataUpdates
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}