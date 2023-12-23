using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotanalytics", "run-pipeline-activity")]
public record AwsIotanalyticsRunPipelineActivityOptions(
[property: CommandSwitch("--pipeline-activity")] string PipelineActivity,
[property: CommandSwitch("--payloads")] string[] Payloads
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}