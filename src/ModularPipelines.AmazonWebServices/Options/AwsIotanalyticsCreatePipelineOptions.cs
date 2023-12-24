using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("iotanalytics", "create-pipeline")]
public record AwsIotanalyticsCreatePipelineOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName,
[property: CommandSwitch("--pipeline-activities")] string[] PipelineActivities
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}