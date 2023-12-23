using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("appstream", "update-fleet")]
public record AwsAppstreamUpdateFleetOptions : AwsOptions
{
    [CommandSwitch("--image-name")]
    public string? ImageName { get; set; }

    [CommandSwitch("--image-arn")]
    public string? ImageArn { get; set; }

    [CommandSwitch("--name")]
    public string? Name { get; set; }

    [CommandSwitch("--instance-type")]
    public string? InstanceType { get; set; }

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

    [CommandSwitch("--idle-disconnect-timeout-in-seconds")]
    public int? IdleDisconnectTimeoutInSeconds { get; set; }

    [CommandSwitch("--attributes-to-delete")]
    public string[]? AttributesToDelete { get; set; }

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