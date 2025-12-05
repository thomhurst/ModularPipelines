using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mwaa", "create-environment")]
public record AwsMwaaCreateEnvironmentOptions(
[property: CliOption("--dag-s3-path")] string DagS3Path,
[property: CliOption("--execution-role-arn")] string ExecutionRoleArn,
[property: CliOption("--name")] string Name,
[property: CliOption("--network-configuration")] string NetworkConfiguration,
[property: CliOption("--source-bucket-arn")] string SourceBucketArn
) : AwsOptions
{
    [CliOption("--airflow-configuration-options")]
    public IEnumerable<KeyValue>? AirflowConfigurationOptions { get; set; }

    [CliOption("--airflow-version")]
    public string? AirflowVersion { get; set; }

    [CliOption("--endpoint-management")]
    public string? EndpointManagement { get; set; }

    [CliOption("--environment-class")]
    public string? EnvironmentClass { get; set; }

    [CliOption("--kms-key")]
    public string? KmsKey { get; set; }

    [CliOption("--logging-configuration")]
    public string? LoggingConfiguration { get; set; }

    [CliOption("--max-workers")]
    public int? MaxWorkers { get; set; }

    [CliOption("--min-workers")]
    public int? MinWorkers { get; set; }

    [CliOption("--plugins-s3-object-version")]
    public string? PluginsS3ObjectVersion { get; set; }

    [CliOption("--plugins-s3-path")]
    public string? PluginsS3Path { get; set; }

    [CliOption("--requirements-s3-object-version")]
    public string? RequirementsS3ObjectVersion { get; set; }

    [CliOption("--requirements-s3-path")]
    public string? RequirementsS3Path { get; set; }

    [CliOption("--schedulers")]
    public int? Schedulers { get; set; }

    [CliOption("--startup-script-s3-object-version")]
    public string? StartupScriptS3ObjectVersion { get; set; }

    [CliOption("--startup-script-s3-path")]
    public string? StartupScriptS3Path { get; set; }

    [CliOption("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CliOption("--webserver-access-mode")]
    public string? WebserverAccessMode { get; set; }

    [CliOption("--weekly-maintenance-window-start")]
    public string? WeeklyMaintenanceWindowStart { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}