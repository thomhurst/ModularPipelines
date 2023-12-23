using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "register-instance-event-notification-attributes")]
public record AwsEc2RegisterInstanceEventNotificationAttributesOptions(
[property: CommandSwitch("--instance-tag-attribute")] string InstanceTagAttribute
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}