using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "describe-auto-scaling-configuration")]
public record AwsApprunnerDescribeAutoScalingConfigurationOptions(
[property: CliOption("--auto-scaling-configuration-arn")] string AutoScalingConfigurationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}