using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("mgn", "remove-template-action")]
public record AwsMgnRemoveTemplateActionOptions(
[property: CommandSwitch("--action-id")] string ActionId,
[property: CommandSwitch("--launch-configuration-template-id")] string LaunchConfigurationTemplateId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}