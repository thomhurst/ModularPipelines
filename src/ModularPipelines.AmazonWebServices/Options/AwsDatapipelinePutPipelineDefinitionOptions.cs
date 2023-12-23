using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "put-pipeline-definition")]
public record AwsDatapipelinePutPipelineDefinitionOptions(
[property: CommandSwitch("--pipeline-id")] string PipelineId,
[property: CommandSwitch("--pipeline-definition")] string PipelineDefinition
) : AwsOptions
{
    [CommandSwitch("--parameter-objects")]
    public string? ParameterObjects { get; set; }

    [CommandSwitch("--parameter-values")]
    public string? ParameterValues { get; set; }

    [CommandSwitch("--parameter-values-uri")]
    public string? ParameterValuesUri { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}