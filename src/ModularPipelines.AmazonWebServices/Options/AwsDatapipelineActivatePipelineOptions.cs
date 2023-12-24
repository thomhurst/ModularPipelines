using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "activate-pipeline")]
public record AwsDatapipelineActivatePipelineOptions(
[property: CommandSwitch("--pipeline-id")] string PipelineId
) : AwsOptions
{
    [CommandSwitch("--parameter-values")]
    public string? ParameterValues { get; set; }

    [CommandSwitch("--start-timestamp")]
    public long? StartTimestamp { get; set; }

    [CommandSwitch("--parameter-values-uri")]
    public string? ParameterValuesUri { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}