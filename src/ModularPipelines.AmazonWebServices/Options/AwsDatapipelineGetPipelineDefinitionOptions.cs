using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "get-pipeline-definition")]
public record AwsDatapipelineGetPipelineDefinitionOptions(
[property: CommandSwitch("--pipeline-id")] string PipelineId
) : AwsOptions
{
    [CommandSwitch("--pipeline-version")]
    public string? PipelineVersion { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}