using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("datapipeline", "evaluate-expression")]
public record AwsDatapipelineEvaluateExpressionOptions(
[property: CliOption("--pipeline-id")] string PipelineId,
[property: CliOption("--object-id")] string ObjectId,
[property: CliOption("--expression")] string Expression
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}