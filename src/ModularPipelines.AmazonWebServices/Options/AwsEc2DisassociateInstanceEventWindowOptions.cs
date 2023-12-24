using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "disassociate-instance-event-window")]
public record AwsEc2DisassociateInstanceEventWindowOptions(
[property: CommandSwitch("--instance-event-window-id")] string InstanceEventWindowId,
[property: CommandSwitch("--association-target")] string AssociationTarget
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}