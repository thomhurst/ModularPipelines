using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "batch-evaluate-feature")]
public record AwsEvidentlyBatchEvaluateFeatureOptions(
[property: CommandSwitch("--project")] string Project,
[property: CommandSwitch("--requests")] string[] Requests
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}