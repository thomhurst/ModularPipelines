using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medical-imaging", "delete-image-set")]
public record AwsMedicalImagingDeleteImageSetOptions(
[property: CommandSwitch("--datastore-id")] string DatastoreId,
[property: CommandSwitch("--image-set-id")] string ImageSetId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}