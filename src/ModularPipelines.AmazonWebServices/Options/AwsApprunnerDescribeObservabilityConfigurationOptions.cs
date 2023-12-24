using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "describe-observability-configuration")]
public record AwsApprunnerDescribeObservabilityConfigurationOptions(
[property: CommandSwitch("--observability-configuration-arn")] string ObservabilityConfigurationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}