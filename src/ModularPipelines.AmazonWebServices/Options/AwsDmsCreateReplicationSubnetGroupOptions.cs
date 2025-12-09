using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "create-replication-subnet-group")]
public record AwsDmsCreateReplicationSubnetGroupOptions(
[property: CliOption("--replication-subnet-group-identifier")] string ReplicationSubnetGroupIdentifier,
[property: CliOption("--replication-subnet-group-description")] string ReplicationSubnetGroupDescription,
[property: CliOption("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}