using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2-instance-connect", "send-ssh-public-key")]
public record AwsEc2InstanceConnectSendSshPublicKeyOptions(
[property: CliOption("--instance-id")] string InstanceId,
[property: CliOption("--instance-os-user")] string InstanceOsUser,
[property: CliOption("--ssh-public-key")] string SshPublicKey
) : AwsOptions
{
    [CliOption("--availability-zone")]
    public string? AvailabilityZone { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}