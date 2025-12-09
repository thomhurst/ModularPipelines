using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medical-imaging", "get-image-set")]
public record AwsMedicalImagingGetImageSetOptions(
[property: CliOption("--datastore-id")] string DatastoreId,
[property: CliOption("--image-set-id")] string ImageSetId
) : AwsOptions
{
    [CliOption("--version-id")]
    public string? VersionId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}