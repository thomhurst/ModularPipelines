using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "wait", "step-complete")]
public record AwsEmrWaitStepCompleteOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--step-id")] string StepId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}