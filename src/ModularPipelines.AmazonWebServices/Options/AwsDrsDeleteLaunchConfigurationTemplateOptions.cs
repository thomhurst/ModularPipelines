using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("drs", "delete-launch-configuration-template")]
public record AwsDrsDeleteLaunchConfigurationTemplateOptions(
[property: CommandSwitch("--launch-configuration-template-id")] string LaunchConfigurationTemplateId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}