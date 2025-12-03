using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2-instance-connect", "ssh")]
public record AwsEc2InstanceConnectSshOptions(
[property: CliOption("--instance-id")] string InstanceId
) : AwsOptions
{
    [CliOption("--instance-ip")]
    public string? InstanceIp { get; set; }

    [CliOption("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CliOption("--os-user")]
    public string? OsUser { get; set; }

    [CliOption("--ssh-port")]
    public int? SshPort { get; set; }

    [CliOption("--local-forwarding")]
    public string? LocalForwarding { get; set; }

    [CliOption("--connection-type")]
    public string? ConnectionType { get; set; }

    [CliOption("--eice-options")]
    public string? EiceOptions { get; set; }
}