using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "modify-replication-subnet-group")]
public record AwsDmsModifyReplicationSubnetGroupOptions(
[property: CommandSwitch("--replication-subnet-group-identifier")] string ReplicationSubnetGroupIdentifier,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--replication-subnet-group-description")]
    public string? ReplicationSubnetGroupDescription { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}