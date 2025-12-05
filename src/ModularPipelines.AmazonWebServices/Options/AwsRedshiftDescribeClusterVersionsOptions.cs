using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "describe-cluster-versions")]
public record AwsRedshiftDescribeClusterVersionsOptions : AwsOptions
{
    [CliOption("--cluster-version")]
    public string? ClusterVersion { get; set; }

    [CliOption("--cluster-parameter-group-family")]
    public string? ClusterParameterGroupFamily { get; set; }

    [CliOption("--starting-token")]
    public string? StartingToken { get; set; }

    [CliOption("--page-size")]
    public int? PageSize { get; set; }

    [CliOption("--max-items")]
    public int? MaxItems { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}