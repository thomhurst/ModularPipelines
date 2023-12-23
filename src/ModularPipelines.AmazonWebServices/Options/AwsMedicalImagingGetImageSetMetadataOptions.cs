using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medical-imaging", "get-image-set-metadata")]
public record AwsMedicalImagingGetImageSetMetadataOptions(
[property: CommandSwitch("--datastore-id")] string DatastoreId,
[property: CommandSwitch("--image-set-id")] string ImageSetId
) : AwsOptions
{
    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }
}