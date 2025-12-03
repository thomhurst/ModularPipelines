using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("healthlake", "start-fhir-import-job")]
public record AwsHealthlakeStartFhirImportJobOptions(
[property: CliOption("--input-data-config")] string InputDataConfig,
[property: CliOption("--job-output-data-config")] string JobOutputDataConfig,
[property: CliOption("--datastore-id")] string DatastoreId,
[property: CliOption("--data-access-role-arn")] string DataAccessRoleArn
) : AwsOptions
{
    [CliOption("--job-name")]
    public string? JobName { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}