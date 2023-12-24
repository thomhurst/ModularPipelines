using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("evidently", "evaluate-feature")]
public record AwsEvidentlyEvaluateFeatureOptions(
[property: CommandSwitch("--entity-id")] string EntityId,
[property: CommandSwitch("--feature")] string Feature,
[property: CommandSwitch("--project")] string Project
) : AwsOptions
{
    [CommandSwitch("--evaluation-context")]
    public string? EvaluationContext { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}