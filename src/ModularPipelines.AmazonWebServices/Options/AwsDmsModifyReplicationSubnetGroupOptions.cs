using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "modify-replication-subnet-group")]
public record AwsDmsModifyReplicationSubnetGroupOptions(
[property: CliOption("--replication-subnet-group-identifier")] string ReplicationSubnetGroupIdentifier,
[property: CliOption("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CliOption("--replication-subnet-group-description")]
    public string? ReplicationSubnetGroupDescription { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}