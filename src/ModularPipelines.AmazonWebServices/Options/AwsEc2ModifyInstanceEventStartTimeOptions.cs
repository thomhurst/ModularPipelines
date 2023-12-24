using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "modify-instance-event-start-time")]
public record AwsEc2ModifyInstanceEventStartTimeOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--instance-event-id")] string InstanceEventId,
[property: CommandSwitch("--not-before")] long NotBefore
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}