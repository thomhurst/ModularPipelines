using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("medical-imaging", "start-dicom-import-job")]
public record AwsMedicalImagingStartDicomImportJobOptions(
[property: CliOption("--data-access-role-arn")] string DataAccessRoleArn,
[property: CliOption("--datastore-id")] string DatastoreId,
[property: CliOption("--input-s3-uri")] string InputS3Uri,
[property: CliOption("--output-s3-uri")] string OutputS3Uri
) : AwsOptions
{
    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}