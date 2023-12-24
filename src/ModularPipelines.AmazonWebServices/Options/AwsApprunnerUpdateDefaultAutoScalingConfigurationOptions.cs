using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "update-default-auto-scaling-configuration")]
public record AwsApprunnerUpdateDefaultAutoScalingConfigurationOptions(
[property: CommandSwitch("--auto-scaling-configuration-arn")] string AutoScalingConfigurationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}