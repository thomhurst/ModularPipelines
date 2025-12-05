using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("gamelift", "create-fleet")]
public record AwsGameliftCreateFleetOptions(
[property: CliOption("--name")] string Name
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--build-id")]
    public string? BuildId { get; set; }

    [CliOption("--script-id")]
    public string? ScriptId { get; set; }

    [CliOption("--server-launch-path")]
    public string? ServerLaunchPath { get; set; }

    [CliOption("--server-launch-parameters")]
    public string? ServerLaunchParameters { get; set; }

    [CliOption("--log-paths")]
    public string[]? LogPaths { get; set; }

    [CliOption("--ec2-instance-type")]
    public string? Ec2InstanceType { get; set; }

    [CliOption("--ec2-inbound-permissions")]
    public string[]? Ec2InboundPermissions { get; set; }

    [CliOption("--new-game-session-protection-policy")]
    public string? NewGameSessionProtectionPolicy { get; set; }

    [CliOption("--runtime-configuration")]
    public string? RuntimeConfiguration { get; set; }

    [CliOption("--resource-creation-limit-policy")]
    public string? ResourceCreationLimitPolicy { get; set; }

    [CliOption("--metric-groups")]
    public string[]? MetricGroups { get; set; }

    [CliOption("--peer-vpc-aws-account-id")]
    public string? PeerVpcAwsAccountId { get; set; }

    [CliOption("--peer-vpc-id")]
    public string? PeerVpcId { get; set; }

    [CliOption("--fleet-type")]
    public string? FleetType { get; set; }

    [CliOption("--instance-role-arn")]
    public string? InstanceRoleArn { get; set; }

    [CliOption("--certificate-configuration")]
    public string? CertificateConfiguration { get; set; }

    [CliOption("--locations")]
    public string[]? Locations { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--compute-type")]
    public string? ComputeType { get; set; }

    [CliOption("--anywhere-configuration")]
    public string? AnywhereConfiguration { get; set; }

    [CliOption("--instance-role-credentials-provider")]
    public string? InstanceRoleCredentialsProvider { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}