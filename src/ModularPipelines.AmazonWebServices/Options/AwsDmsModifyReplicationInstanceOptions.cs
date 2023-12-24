using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "modify-replication-instance")]
public record AwsDmsModifyReplicationInstanceOptions(
[property: CommandSwitch("--replication-instance-arn")] string ReplicationInstanceArn
) : AwsOptions
{
    [CommandSwitch("--allocated-storage")]
    public int? AllocatedStorage { get; set; }

    [CommandSwitch("--replication-instance-class")]
    public string? ReplicationInstanceClass { get; set; }

    [CommandSwitch("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CommandSwitch("--preferred-maintenance-window")]
    public string? PreferredMaintenanceWindow { get; set; }

    [CommandSwitch("--engine-version")]
    public string? EngineVersion { get; set; }

    [CommandSwitch("--replication-instance-identifier")]
    public string? ReplicationInstanceIdentifier { get; set; }

    [CommandSwitch("--network-type")]
    public string? NetworkType { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}