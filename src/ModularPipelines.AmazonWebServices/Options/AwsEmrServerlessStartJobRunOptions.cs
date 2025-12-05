using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr-serverless", "start-job-run")]
public record AwsEmrServerlessStartJobRunOptions(
[property: CliOption("--application-id")] string ApplicationId,
[property: CliOption("--execution-role-arn")] string ExecutionRoleArn
) : AwsOptions
{
    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--job-driver")]
    public string? JobDriver { get; set; }

    [CliOption("--configuration-overrides")]
    public string? ConfigurationOverrides { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--execution-timeout-minutes")]
    public long? ExecutionTimeoutMinutes { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}