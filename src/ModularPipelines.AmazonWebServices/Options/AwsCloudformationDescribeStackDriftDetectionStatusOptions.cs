using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("cloudformation", "describe-stack-drift-detection-status")]
public record AwsCloudformationDescribeStackDriftDetectionStatusOptions(
[property: CommandSwitch("--stack-drift-detection-id")] string StackDriftDetectionId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}