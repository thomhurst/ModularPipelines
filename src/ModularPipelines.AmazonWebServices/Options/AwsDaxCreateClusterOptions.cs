using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dax", "create-cluster")]
public record AwsDaxCreateClusterOptions(
[property: CliOption("--cluster-name")] string ClusterName,
[property: CliOption("--node-type")] string NodeType,
[property: CliOption("--replication-factor")] int ReplicationFactor,
[property: CliOption("--iam-role-arn")] string IamRoleArn
) : AwsOptions
{
    [CliOption("--description")]
    public string? Description { get; set; }

    [CliOption("--availability-zones")]
    public string[]? AvailabilityZones { get; set; }

    [CliOption("--subnet-group-name")]
    public string? SubnetGroupName { get; set; }

    [CliOption("--security-group-ids")]
    public string[]? SecurityGroupIds { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--notification-topic-arn")]
    public string? NotificationTopicArn { get; set; }

    [CliOption("--parameter-group-name")]
    public string? ParameterGroupName { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--sse-specification")]
    public string? SseSpecification { get; set; }

    [CliOption("--cluster-endpoint-encryption-type")]
    public string? ClusterEndpointEncryptionType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}