using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "create-replication-subnet-group")]
public record AwsDmsCreateReplicationSubnetGroupOptions(
[property: CommandSwitch("--replication-subnet-group-identifier")] string ReplicationSubnetGroupIdentifier,
[property: CommandSwitch("--replication-subnet-group-description")] string ReplicationSubnetGroupDescription,
[property: CommandSwitch("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CommandSwitch("--tags")]
    public string[]? Tags { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}