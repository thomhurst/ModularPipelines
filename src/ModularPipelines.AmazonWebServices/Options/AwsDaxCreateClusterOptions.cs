using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dax", "create-cluster")]
public record AwsDaxCreateClusterOptions(
[property: CommandSwitch("--cluster-name")] string ClusterName,
[property: CommandSwitch("--node-type")] string NodeType,
[property: CommandSwitch("--replication-factor")] int ReplicationFactor,
[property: CommandSwitch("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CommandSwitch("--description")]
    public string? Description { get; set; }

    [CommandSwitch("--availability-zones")]
    public string[]? AvailabilityZones { get; set; }

    [CommandSwitch("--subnet-group-name")]
    public string? SubnetGroupName { get; set; }

    [CommandSwitch("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--notification-topic-arn")]
    public string? NotificationTopicArn { get; set; }

    [CommandSwitch("--parameter-group-name")]
    public string? ParameterGroupName { get; set; }

    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CommandSwitch("--cluster-endpoint-encryption-type")]
    public string? ClusterEndpointEncryptionType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}