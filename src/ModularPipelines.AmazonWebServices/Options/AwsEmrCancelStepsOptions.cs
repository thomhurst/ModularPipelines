using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("emr", "cancel-steps")]
public record AwsEmrCancelStepsOptions(
[property: CliOption("--cluster-id")] string ClusterId,
[property: CliOption("--step-ids")] string[] StepIds
) : AwsOptions
{
    [CliOption("--step-cancellation-option")]
    public string? StepCancellationOption { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}