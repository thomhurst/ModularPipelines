using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;
using ModularPipelines.Models;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "create-fleet")]
public record AwsAppstreamCreateFleetOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--instance-type")] string InstanceType
) : AwsOptions
{
    [CommandSwitch("--image-name")]
    public string? ImageName { get; set; }

    [CommandSwitch("--image-arn")]
    public string? ImageArn { get; set; }

    [CommandSwitch("--fleet-type")]
    public string? FleetType { get; set; }

    [CommandSwitch("--compute-capacity")]
    public string? ComputeCapacity { get; set; }

    [CommandSwitch("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CommandSwitch("--max-user-duration-in-seconds")]
    public int? MaxUserDurationInSeconds { get; set; }

    [CommandSwitch("--disconnect-timeout-in-seconds")]
    public int? DisconnectTimeoutInSeconds { get; set; }

    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--display-name")]
    public string? DisplayName { get; set; }

    [CommandSwitch("--domain-join-info")]
    public string? DomainJoinInfo { get; set; }

    [CommandSwitch("--tags")]
    public IEnumerable<KeyValue>? Tags { get; set; }

    [CommandSwitch("--idle-disconnect-timeout-in-seconds")]
    public int? IdleDisconnectTimeoutInSeconds { get; set; }

    [CommandSwitch("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CommandSwitch("--stream-view")]
    public string? StreamView { get; set; }

    [CommandSwitch("--platform")]
    public string? Platform { get; set; }

    [CommandSwitch("--max-concurrent-sessions")]
    public int? MaxConcurrentSessions { get; set; }

    [CommandSwitch("--usb-device-filter-strings")]
    public string[]? UsbDeviceFilterStrings { get; set; }

    [CommandSwitch("--session-script-s3-location")]
    public string? SessionScriptS3Location { get; set; }

    [CommandSwitch("--max-sessions-per-instance")]
    public int? MaxSessionsPerInstance { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}