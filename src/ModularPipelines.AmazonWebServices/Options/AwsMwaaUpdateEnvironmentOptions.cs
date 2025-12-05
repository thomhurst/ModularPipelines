using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("mwaa", "update-environment")]
public record AwsMwaaUpdateEnvironmentOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--airflow-configuration-options")]
    public IEnumerable<KeyValue>? AirflowConfigurationOptions { get; set; }

    [CliOption("--airflow-version")]
    public string? AirflowVersion { get; set; }

    [CliOption("--dag-s3-path")]
    public string? DagS3Path { get; set; }

    [CliOption("--environment-class")]
    public string? EnvironmentClass { get; set; }

    [CliOption("--execution-role-arn")]
    public string? ExecutionRoleArn { get; set; }

    [CliOption("--logging-configuration")]
    public string? LoggingConfiguration { get; set; }

    [CliOption("--max-workers")]
    public int? MaxWorkers { get; set; }

    [CliOption("--min-workers")]
    public int? MinWorkers { get; set; }

    [CliOption("--network-configuration")]
    public string? NetworkConfiguration { get; set; }

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

    [CliOption("--source-bucket-arn")]
    public string? SourceBucketArn { get; set; }

    [CliOption("--startup-script-s3-object-version")]
    public string? StartupScriptS3ObjectVersion { get; set; }

    [CliOption("--startup-script-s3-path")]
    public string? StartupScriptS3Path { get; set; }

    [CliOption("--webserver-access-mode")]
    public string? WebserverAccessMode { get; set; }

    [CliOption("--weekly-maintenance-window-start")]
    public string? WeeklyMaintenanceWindowStart { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}