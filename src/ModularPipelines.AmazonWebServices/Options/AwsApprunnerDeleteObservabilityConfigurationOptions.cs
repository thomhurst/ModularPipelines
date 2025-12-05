using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("apprunner", "delete-observability-configuration")]
public record AwsApprunnerDeleteObservabilityConfigurationOptions(
[property: CliOption("--observability-configuration-arn")] string ObservabilityConfigurationArn
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}