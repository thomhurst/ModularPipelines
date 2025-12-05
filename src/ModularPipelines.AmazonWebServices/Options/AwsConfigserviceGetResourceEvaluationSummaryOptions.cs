using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("configservice", "get-resource-evaluation-summary")]
public record AwsConfigserviceGetResourceEvaluationSummaryOptions(
[property: CliOption("--resource-evaluation-id")] string ResourceEvaluationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}