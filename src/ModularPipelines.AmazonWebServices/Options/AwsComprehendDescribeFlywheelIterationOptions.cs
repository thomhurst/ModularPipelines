using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("comprehend", "describe-flywheel-iteration")]
public record AwsComprehendDescribeFlywheelIterationOptions(
[property: CliOption("--flywheel-arn")] string FlywheelArn,
[property: CliOption("--flywheel-iteration-id")] string FlywheelIterationId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}