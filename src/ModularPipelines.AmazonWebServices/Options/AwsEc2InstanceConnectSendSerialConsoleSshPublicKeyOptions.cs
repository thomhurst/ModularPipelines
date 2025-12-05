using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2-instance-connect", "send-serial-console-ssh-public-key")]
public record AwsEc2InstanceConnectSendSerialConsoleSshPublicKeyOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--ssh-public-key")] string SshPublicKey
) : AwsOptions
{
    [CliOption("--serial-port")]
    public int? SerialPort { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}