using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("connect", "update-contact-flow-module-content")]
public record AwsConnectUpdateContactFlowModuleContentOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--contact-flow-module-id")] string ContactFlowModuleId,
[property: CommandSwitch("--content")] string Content
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}