using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medical-imaging", "get-image-frame")]
public record AwsMedicalImagingGetImageFrameOptions(
[property: CliOption("--datastore-id")] string DatastoreId,
[property: CliOption("--image-set-id")] string ImageSetId,
[property: CliOption("--image-frame-information")] string ImageFrameInformation
) : AwsOptions;