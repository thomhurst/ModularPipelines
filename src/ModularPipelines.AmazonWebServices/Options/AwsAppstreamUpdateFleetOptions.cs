using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("appstream", "update-fleet")]
public record AwsAppstreamUpdateFleetOptions : AwsOptions
{
    [CliOption("--image-name")]
    public string? ImageName { get; set; }

    [CliOption("--image-arn")]
    public string? ImageArn { get; set; }

    [CliOption("--name")]
    public string? Name { get; set; }

    [CliOption("--instance-type")]
    public string? InstanceType { get; set; }

    [CliOption("--compute-capacity")]
    public string? ComputeCapacity { get; set; }

    [CliOption("--vpc-config")]
    public string? VpcConfig { get; set; }

    [CliOption("--max-user-duration-in-seconds")]
    public int? MaxUserDurationInSeconds { get; set; }

    [CliOption("--disconnect-timeout-in-seconds")]
    public int? DisconnectTimeoutInSeconds { get; set; }

    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--display-name")]
    public string? DisplayName { get; set; }

    [CliOption("--domain-join-info")]
    public string? DomainJoinInfo { get; set; }

    [CliOption("--idle-disconnect-timeout-in-seconds")]
    public int? IdleDisconnectTimeoutInSeconds { get; set; }

    [CliOption("--attributes-to-delete")]
    public string[]? AttributesToDelete { get; set; }

    [CliOption("--iam-role-arn")]
    public string? IamRoleArn { get; set; }

    [CliOption("--stream-view")]
    public string? StreamView { get; set; }

    [CliOption("--platform")]
    public string? Platform { get; set; }

    [CliOption("--max-concurrent-sessions")]
    public int? MaxConcurrentSessions { get; set; }

    [CliOption("--usb-device-filter-strings")]
    public string[]? UsbDeviceFilterStrings { get; set; }

    [CliOption("--session-script-s3-location")]
    public string? SessionScriptS3Location { get; set; }

    [CliOption("--max-sessions-per-instance")]
    public int? MaxSessionsPerInstance { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}