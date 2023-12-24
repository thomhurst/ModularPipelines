using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elastictranscoder", "list-jobs-by-pipeline")]
public record AwsElastictranscoderListJobsByPipelineOptions(
[property: CommandSwitch("--pipeline-id")] string PipelineId
) : AwsOptions
{
    [CommandSwitch("--ascending")]
    public string? Ascending { get; set; }

    [CommandSwitch("--starting-token")]
    public string? StartingToken { get; set; }

    [CommandSwitch("--max-items")]
    public int? MaxItems { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}