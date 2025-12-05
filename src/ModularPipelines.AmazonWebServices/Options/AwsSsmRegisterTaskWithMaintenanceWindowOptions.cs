using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "register-task-with-maintenance-window")]
public record AwsSsmRegisterTaskWithMaintenanceWindowOptions(
[property: CliOption("--window-id")] string WindowId,
[property: CliOption("--task-arn")] string TaskArn,
[property: CliOption("--task-type")] string TaskType
) : AwsOptions
{
    [CliOption("--targets")]
    public string[]? Targets { get; set; }

    [CliOption("--service-role-arn")]
    public string? ServiceRoleArn { get; set; }

    [CliOption("--task-parameters")]
    public IEnumerable<KeyValue>? TaskParameters { get; set; }

    [CliOption("--task-invocation-parameters")]
    public string? TaskInvocationParameters { get; set; }

    [CliOption("--priority")]
    public int? Priority { get; set; }

    [CliOption("--max-concurrency")]
    public string? MaxConcurrency { get; set; }

    [CliOption("--max-errors")]
    public string? MaxErrors { get; set; }

    [CliOption("--logging-info")]
    public string? LoggingInfo { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--client-token")]
    public string? ClientToken { get; set; }

    [CliOption("--cutoff-behavior")]
    public string? CutoffBehavior { get; set; }

    [CliOption("--alarm-configuration")]
    public string? AlarmConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}