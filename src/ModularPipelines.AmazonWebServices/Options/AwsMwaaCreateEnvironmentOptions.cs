using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mwaa", "create-environment")]
public record AwsMwaaCreateEnvironmentOptions(
[property: CommandSwitch("--dag-s3-path")] string DagS3Path,
[property: CommandSwitch("--execution-role-arn")] string ExecutionRoleArn,
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--network-configuration")] string NetworkConfiguration,
[property: CommandSwitch("--source-bucket-arn")] string SourceBucketArn
) : AwsOptions
{
    [CommandSwitch("--airflow-configuration-options")]
    public IEnumerable<KeyValue>? AirflowConfigurationOptions { get; set; }

    [CommandSwitch("--airflow-version")]
    public string? AirflowVersion { get; set; }

    [CommandSwitch("--endpoint-management")]
    public string? EndpointManagement { get; set; }

    [CommandSwitch("--environment-class")]
    public string? EnvironmentClass { get; set; }

    [CommandSwitch("--kms-key")]
    public string? KmsKey { get; set; }

    [CommandSwitch("--logging-configuration")]
    public string? LoggingConfiguration { get; set; }

    [CommandSwitch("--max-workers")]
    public int? MaxWorkers { get; set; }

    [CommandSwitch("--min-workers")]
    public int? MinWorkers { get; set; }

    [CommandSwitch("--plugins-s3-object-version")]
    public string? PluginsS3ObjectVersion { get; set; }

    [CommandSwitch("--plugins-s3-path")]
    public string? PluginsS3Path { get; set; }

    [CommandSwitch("--requirements-s3-object-version")]
    public string? RequirementsS3ObjectVersion { get; set; }

    [CommandSwitch("--requirements-s3-path")]
    public string? RequirementsS3Path { get; set; }

    [CommandSwitch("--schedulers")]
    public int? Schedulers { get; set; }

    [CommandSwitch("--startup-script-s3-object-version")]
    public string? StartupScriptS3ObjectVersion { get; set; }

    [CommandSwitch("--startup-script-s3-path")]
    public string? StartupScriptS3Path { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--webserver-access-mode")]
    public string? WebserverAccessMode { get; set; }

    [CommandSwitch("--weekly-maintenance-window-start")]
    public string? WeeklyMaintenanceWindowStart { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}