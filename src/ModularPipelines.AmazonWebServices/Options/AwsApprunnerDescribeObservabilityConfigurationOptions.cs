using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "describe-observability-configuration")]
public record AwsApprunnerDescribeObservabilityConfigurationOptions(
[property: CliOption("--observability-configuration-arn")] string ObservabilityConfigurationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}