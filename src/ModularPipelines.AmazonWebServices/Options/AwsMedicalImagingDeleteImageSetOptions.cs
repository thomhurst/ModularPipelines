using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medical-imaging", "delete-image-set")]
public record AwsMedicalImagingDeleteImageSetOptions(
[property: CliOption("--datastore-id")] string DatastoreId,
[property: CliOption("--image-set-id")] string ImageSetId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}