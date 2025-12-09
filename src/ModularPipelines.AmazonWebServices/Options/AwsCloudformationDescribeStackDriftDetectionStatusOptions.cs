using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("cloudformation", "describe-stack-drift-detection-status")]
public record AwsCloudformationDescribeStackDriftDetectionStatusOptions(
[property: CliOption("--stack-drift-detection-id")] string StackDriftDetectionId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}