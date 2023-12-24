using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "report-task-runner-heartbeat")]
public record AwsDatapipelineReportTaskRunnerHeartbeatOptions(
[property: CommandSwitch("--taskrunner-id")] string TaskrunnerId
) : AwsOptions
{
    [CommandSwitch("--worker-group")]
    public string? WorkerGroup { get; set; }

    [CommandSwitch("--hostname")]
    public string? Hostname { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}