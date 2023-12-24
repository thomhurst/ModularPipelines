using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("dms", "delete-replication-subnet-group")]
public record AwsDmsDeleteReplicationSubnetGroupOptions(
[property: CommandSwitch("--replication-subnet-group-identifier")] string ReplicationSubnetGroupIdentifier
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}