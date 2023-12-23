using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medical-imaging", "get-image-set")]
public record AwsMedicalImagingGetImageSetOptions(
[property: CommandSwitch("--datastore-id")] string DatastoreId,
[property: CommandSwitch("--image-set-id")] string ImageSetId
) : AwsOptions
{
    [CommandSwitch("--version-id")]
    public string? VersionId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}