using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("amplifybackend", "list-backend-jobs")]
public record AwsAmplifybackendListBackendJobsOptions(
[property: CliOption("--app-id")] string AppId,
[property: CliOption("--backend-environment-name")] string BackendEnvironmentName
) : AwsOptions
{
    [CliOption("--job-id")]
    public string? JobId { get; set; }

    [CliOption("--operation")]
    public string? Operation { get; set; }

    [CliOption("--status")]
    public string? Status { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}