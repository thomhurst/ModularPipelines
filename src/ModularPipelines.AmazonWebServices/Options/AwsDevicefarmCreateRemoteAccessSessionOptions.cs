using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("devicefarm", "create-remote-access-session")]
public record AwsDevicefarmCreateRemoteAccessSessionOptions(
[property: CommandSwitch("--project-arn")] string ProjectArn,
[property: CommandSwitch("--device-arn")] string DeviceArn
) : AwsOptions
{
    [CommandSwitch("--instance-arn")]
    public string? InstanceArn { get; set; }

    [CommandSwitch("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CommandSwitch("--remote-record-app-arn")]
    public string? RemoteRecordAppArn { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--client-id")]
    public string? ClientId { get; set; }

    [CommandSwitch("--configuration")]
    public string? Configuration { get; set; }

    [CommandSwitch("--interaction-mode")]
    public string? InteractionMode { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}