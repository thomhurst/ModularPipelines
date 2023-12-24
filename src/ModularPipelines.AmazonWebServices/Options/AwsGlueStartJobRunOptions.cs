using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("glue", "start-job-run")]
public record AwsGlueStartJobRunOptions(
[property: CommandSwitch("--job-name")] string JobName
) : AwsOptions
{
    [CommandSwitch("--job-run-id")]
    public string? JobRunId { get; set; }

    [CommandSwitch("--arguments")]
    public IEnumerable<KeyValue>? AwsGlueArguments { get; set; }

    [CommandSwitch("--allocated-capacity")]
    public int? AllocatedCapacity { get; set; }

    [CommandSwitch("--timeout")]
    public int? Timeout { get; set; }

    [CommandSwitch("--max-capacity")]
    public double? MaxCapacity { get; set; }

    [CommandSwitch("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CommandSwitch("--notification-property")]
    public string? NotificationProperty { get; set; }

    [CommandSwitch("--worker-type")]
    public string? WorkerType { get; set; }

    [CommandSwitch("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CommandSwitch("--execution-class")]
    public string? ExecutionClass { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}