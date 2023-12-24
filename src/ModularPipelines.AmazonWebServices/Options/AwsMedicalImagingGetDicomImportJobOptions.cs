using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("medical-imaging", "get-dicom-import-job")]
public record AwsMedicalImagingGetDicomImportJobOptions(
[property: CommandSwitch("--datastore-id")] string DatastoreId,
[property: CommandSwitch("--job-id")] string JobId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}