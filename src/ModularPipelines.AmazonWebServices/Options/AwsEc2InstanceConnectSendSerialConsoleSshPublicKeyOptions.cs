using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2-instance-connect", "send-serial-console-ssh-public-key")]
public record AwsEc2InstanceConnectSendSerialConsoleSshPublicKeyOptions(
[property: CommandSwitch("--instance-id")] string InstanceId,
[property: CommandSwitch("--ssh-public-key")] string SshPublicKey
) : AwsOptions
{
    [CommandSwitch("--serial-port")]
    public int? SerialPort { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}