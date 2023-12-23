using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2-instance-connect", "send-ssh-public-key")]
public record AwsEc2InstanceConnectSendSshPublicKeyOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--instance-os-user")] string InstanceOsUser,
[property: CommandSwitch("--ssh-public-key")] string SshPublicKey
) : AwsOptions
{
    [CommandSwitch("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}