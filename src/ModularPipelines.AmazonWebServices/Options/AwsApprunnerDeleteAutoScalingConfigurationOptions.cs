using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("apprunner", "delete-auto-scaling-configuration")]
public record AwsApprunnerDeleteAutoScalingConfigurationOptions(
[property: CommandSwitch("--auto-scaling-configuration-arn")] string AutoScalingConfigurationArn
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}