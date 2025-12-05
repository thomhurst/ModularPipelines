using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthlake", "describe-fhir-import-job")]
public record AwsHealthlakeDescribeFhirImportJobOptions(
[property: CliOption("--datastore-id")] string DatastoreId,
[property: CliOption("--job-id")] string JobId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}