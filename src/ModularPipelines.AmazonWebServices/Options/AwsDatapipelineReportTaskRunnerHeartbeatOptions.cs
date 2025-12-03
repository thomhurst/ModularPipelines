using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "report-task-runner-heartbeat")]
public record AwsDatapipelineReportTaskRunnerHeartbeatOptions(
[property: CliOption("--taskrunner-id")] string TaskrunnerId
) : AwsOptions
{
    [CliOption("--worker-group")]
    public string? WorkerGroup { get; set; }

    [CliOption("--hostname")]
    public string? Hostname { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}