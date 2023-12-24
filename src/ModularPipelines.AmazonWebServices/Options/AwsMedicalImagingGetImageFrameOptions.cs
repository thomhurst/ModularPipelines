using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medical-imaging", "get-image-frame")]
public record AwsMedicalImagingGetImageFrameOptions(
[property: CommandSwitch("--datastore-id")] string DatastoreId,
[property: CommandSwitch("--image-set-id")] string ImageSetId,
[property: CommandSwitch("--image-frame-information")] string ImageFrameInformation
) : AwsOptions;