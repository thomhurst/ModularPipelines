using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "modify-db-cluster-endpoint")]
public record AwsNeptuneModifyDbClusterEndpointOptions(
[property: CliOption("--db-cluster-endpoint-identifier")] string DbClusterEndpointIdentifier
) : AwsOptions
{
    [CliOption("--endpoint-type")]
    public string? EndpointType { get; set; }

    [CliOption("--static-members")]
    public string[]? StaticMembers { get; set; }

    [CliOption("--excluded-members")]
    public string[]? ExcludedMembers { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}