using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "validate-pipeline-definition")]
public record AwsDatapipelineValidatePipelineDefinitionOptions(
[property: CommandSwitch("--pipeline-id")] string PipelineId,
[property: CommandSwitch("--pipeline-objects")] string[] PipelineObjects
) : AwsOptions
{
    [CommandSwitch("--parameter-objects")]
    public string[]? ParameterObjects { get; set; }

    [CommandSwitch("--parameter-values")]
    public string[]? ParameterValues { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}