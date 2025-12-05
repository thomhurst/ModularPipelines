using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("neptune", "describe-db-cluster-endpoints")]
public record AwsNeptuneDescribeDbClusterEndpointsOptions : AwsOptions
{
    [CliOption("--db-cluster-identifier")]
    public string? DbClusterIdentifier { get; set; }

    [CliOption("--db-cluster-endpoint-identifier")]
    public string? DbClusterEndpointIdentifier { get; set; }

    [CliOption("--filters")]
    public string[]? Filters { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}