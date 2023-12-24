using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "delete-observability-configuration")]
public record AwsApprunnerDeleteObservabilityConfigurationOptions(
[property: CommandSwitch("--observability-configuration-arn")] string ObservabilityConfigurationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}