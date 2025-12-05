using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "modify-replication-instance")]
public record AwsDmsModifyReplicationInstanceOptions(
[property: CliOption("--replication-instance-arn")] string ReplicationInstanceArn
) : AwsOptions
{
    [CliOption("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CliOption("--replication-instance-class")]
    public string? ReplicationInstanceClass { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CliOption("--engine-version")]
    public string? EngineVersion { get; set; }

    [CliOption("--replication-instance-identifier")]
    public string? ReplicationInstanceIdentifier { get; set; }

    [CliOption("--network-type")]
    public string? NetworkType { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}