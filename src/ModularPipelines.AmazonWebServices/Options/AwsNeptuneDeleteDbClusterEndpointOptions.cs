using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "delete-db-cluster-endpoint")]
public record AwsNeptuneDeleteDbClusterEndpointOptions(
[property: CliOption("--db-cluster-endpoint-identifier")] string DbClusterEndpointIdentifier
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}