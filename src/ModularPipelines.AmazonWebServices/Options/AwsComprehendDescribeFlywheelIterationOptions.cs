using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("comprehend", "describe-flywheel-iteration")]
public record AwsComprehendDescribeFlywheelIterationOptions(
[property: CommandSwitch("--flywheel-arn")] string FlywheelArn,
[property: CommandSwitch("--flywheel-iteration-id")] string FlywheelIterationId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}