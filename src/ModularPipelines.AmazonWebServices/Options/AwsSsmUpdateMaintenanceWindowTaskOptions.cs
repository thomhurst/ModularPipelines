using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ssm", "update-maintenance-window-task")]
public record AwsSsmUpdateMaintenanceWindowTaskOptions(
[property: CliOption("--window-id")] string WindowId,
[property: CliOption("--window-task-id")] string WindowTaskId
) : AwsOptions
{
    [CliOption("--targets")]
    public string[]? Targets { get; set; }

    [CliOption("--task-arn")]
    public string? TaskArn { get; set; }

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

    [CliOption("--cutoff-behavior")]
    public string? CutoffBehavior { get; set; }

    [CliOption("--alarm-configuration")]
    public string? AlarmConfiguration { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}