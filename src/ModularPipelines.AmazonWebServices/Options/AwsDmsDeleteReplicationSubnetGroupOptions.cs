using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("dms", "delete-replication-subnet-group")]
public record AwsDmsDeleteReplicationSubnetGroupOptions(
[property: CliOption("--replication-subnet-group-identifier")] string ReplicationSubnetGroupIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}