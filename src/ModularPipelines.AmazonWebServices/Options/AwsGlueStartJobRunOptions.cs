using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("glue", "start-job-run")]
public record AwsGlueStartJobRunOptions(
[property: CliOption("--job-name")] string JobName
) : AwsOptions
{
    [CliOption("--job-run-id")]
    public string? JobRunId { get; set; }

    [CliOption("--arguments")]
    public IEnumerable<KeyValue>? AwsGlueArguments { get; set; }

    [CliOption("--allocated-capacity")]
    public int? AllocatedCapacity { get; set; }

    [CliOption("--timeout")]
    public int? Timeout { get; set; }

    [CliOption("--max-capacity")]
    public double? MaxCapacity { get; set; }

    [CliOption("--security-configuration")]
    public string? SecurityConfiguration { get; set; }

    [CliOption("--notification-property")]
    public string? NotificationProperty { get; set; }

    [CliOption("--worker-type")]
    public string? WorkerType { get; set; }

    [CliOption("--number-of-workers")]
    public int? NumberOfWorkers { get; set; }

    [CliOption("--execution-class")]
    public string? ExecutionClass { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}