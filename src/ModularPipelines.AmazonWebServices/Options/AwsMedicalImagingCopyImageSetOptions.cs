using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medical-imaging", "copy-image-set")]
public record AwsMedicalImagingCopyImageSetOptions(
[property: CliOption("--datastore-id")] string DatastoreId,
[property: CliOption("--source-image-set-id")] string SourceImageSetId,
[property: CliOption("--copy-image-set-information")] string CopyImageSetInformation
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}