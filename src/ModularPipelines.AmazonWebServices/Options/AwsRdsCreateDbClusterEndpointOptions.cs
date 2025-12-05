using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-db-cluster-endpoint")]
public record AwsRdsCreateDbClusterEndpointOptions(
[property: CliOption("--db-cluster-identifier")] string DbClusterIdentifier,
[property: CliOption("--db-cluster-endpoint-identifier")] string DbClusterEndpointIdentifier,
[property: CliOption("--endpoint-type")] string EndpointType
) : AwsOptions
{
    [CliOption("--static-members")]
    public string[]? StaticMembers { get; set; }

    [CliOption("--excluded-members")]
    public string[]? ExcludedMembers { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}