using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("configservice", "get-resource-evaluation-summary")]
public record AwsConfigserviceGetResourceEvaluationSummaryOptions(
[property: CommandSwitch("--resource-evaluation-id")] string ResourceEvaluationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}