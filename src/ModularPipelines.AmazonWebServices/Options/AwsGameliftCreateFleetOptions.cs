using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("gamelift", "create-fleet")]
public record AwsGameliftCreateFleetOptions(
[property: CommandSwitch("--name")] string Name
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--build-id")]
    public string? BuildId { get; set; }

    [CommandSwitch("--script-id")]
    public string? ScriptId { get; set; }

    [CommandSwitch("--server-launch-path")]
    public string? ServerLaunchPath { get; set; }

    [CommandSwitch("--server-launch-parameters")]
    public string? ServerLaunchParameters { get; set; }

    [CommandSwitch("--log-paths")]
    public string[]? LogPaths { get; set; }

    [CommandSwitch("--ec2-instance-type")]
    public string? Ec2InstanceType { get; set; }

    [CommandSwitch("--ec2-inbound-permissions")]
    public string[]? Ec2InboundPermissions { get; set; }

    [CommandSwitch("--new-game-session-protection-policy")]
    public string? NewGameSessionProtectionPolicy { get; set; }

    [CommandSwitch("--runtime-configuration")]
    public string? RuntimeConfiguration { get; set; }

    [CommandSwitch("--resource-creation-limit-policy")]
    public string? ResourceCreationLimitPolicy { get; set; }

    [CommandSwitch("--metric-groups")]
    public string[]? MetricGroups { get; set; }

    [CommandSwitch("--peer-vpc-aws-account-id")]
    public string? PeerVpcAwsAccountId { get; set; }

    [CommandSwitch("--peer-vpc-id")]
    public string? PeerVpcId { get; set; }

    [CommandSwitch("--fleet-type")]
    public string? FleetType { get; set; }

    [CommandSwitch("--instance-role-arn")]
    public string? InstanceRoleArn { get; set; }

    [CommandSwitch("--certificate-configuration")]
    public string? CertificateConfiguration { get; set; }

    [CommandSwitch("--locations")]
    public string[]? Locations { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--compute-type")]
    public string? ComputeType { get; set; }

    [CommandSwitch("--anywhere-configuration")]
    public string? AnywhereConfiguration { get; set; }

    [CommandSwitch("--instance-role-credentials-provider")]
    public string? InstanceRoleCredentialsProvider { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}