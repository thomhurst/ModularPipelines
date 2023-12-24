using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("healthlake", "start-fhir-import-job")]
public record AwsHealthlakeStartFhirImportJobOptions(
[property: CommandSwitch("--input-data-config")] string InputDataConfig,
[property: CommandSwitch("--job-output-data-config")] string JobOutputDataConfig,
[property: CommandSwitch("--datastore-id")] string DatastoreId,
[property: CommandSwitch("--data-access-role-arn")] string DataAccessRoleArn
) : AwsOptions
{
    [CommandSwitch("--job-name")]
    public string? JobName { get; set; }

    [CommandSwitch("--client-token")]
    public string? ClientToken { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}