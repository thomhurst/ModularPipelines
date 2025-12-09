using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "delete-auto-scaling-configuration")]
public record AwsApprunnerDeleteAutoScalingConfigurationOptions(
[property: CliOption("--auto-scaling-configuration-arn")] string AutoScalingConfigurationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}