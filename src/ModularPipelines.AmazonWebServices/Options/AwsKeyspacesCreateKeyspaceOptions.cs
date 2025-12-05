using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("keyspaces", "create-keyspace")]
public record AwsKeyspacesCreateKeyspaceOptions(
[property: CliOption("--keyspace-name")] string KeyspaceName
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--replication-specification")]
    public string? ReplicationSpecification { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}