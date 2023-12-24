using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("osis", "get-pipeline-change-progress")]
public record AwsOsisGetPipelineChangeProgressOptions(
[property: CommandSwitch("--pipeline-name")] string PipelineName
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}