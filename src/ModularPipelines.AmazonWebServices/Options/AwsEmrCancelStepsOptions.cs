using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("emr", "cancel-steps")]
public record AwsEmrCancelStepsOptions(
[property: CommandSwitch("--cluster-id")] string ClusterId,
[property: CommandSwitch("--step-ids")] string[] StepIds
) : AwsOptions
{
    [CommandSwitch("--step-cancellation-option")]
    public string? StepCancellationOption { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}