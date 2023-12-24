using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("datapipeline", "evaluate-expression")]
public record AwsDatapipelineEvaluateExpressionOptions(
[property: CommandSwitch("--pipeline-id")] string PipelineId,
[property: CommandSwitch("--object-id")] string ObjectId,
[property: CommandSwitch("--expression")] string Expression
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}