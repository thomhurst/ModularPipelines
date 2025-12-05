using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("devicefarm", "create-remote-access-session")]
public record AwsDevicefarmCreateRemoteAccessSessionOptions(
[property: CliOption("--project-arn")] string ProjectArn,
[property: CliOption("--device-arn")] string DeviceArn
) : AwsOptions
{
    [CliOption("--instance-arn")]
    public string? InstanceArn { get; set; }

    [CliOption("--ssh-public-key")]
    public string? SshPublicKey { get; set; }

    [CliOption("--remote-record-app-arn")]
    public string? RemoteRecordAppArn { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--client-id")]
    public string? ClientId { get; set; }

    [CliOption("--configuration")]
    public string? Configuration { get; set; }

    [CliOption("--interaction-mode")]
    public string? InteractionMode { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}