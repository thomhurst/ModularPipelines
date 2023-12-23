using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2-instance-connect", "ssh")]
public record AwsEc2InstanceConnectSshOptions(
[property: CommandSwitch("--instance-id")] string InstanceId
) : AwsOptions
{
    [CommandSwitch("--instance-ip")]
    public string? InstanceIp { get; set; }

    [CommandSwitch("--private-key-file")]
    public string? PrivateKeyFile { get; set; }

    [CommandSwitch("--os-user")]
    public string? OsUser { get; set; }

    [CommandSwitch("--ssh-port")]
    public int? SshPort { get; set; }

    [CommandSwitch("--local-forwarding")]
    public string? LocalForwarding { get; set; }

    [CommandSwitch("--connection-type")]
    public string? ConnectionType { get; set; }

    [CommandSwitch("--eice-options")]
    public string? EiceOptions { get; set; }
}