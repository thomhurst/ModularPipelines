using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "modify-cluster-db-revision")]
public record AwsRedshiftModifyClusterDbRevisionOptions(
[property: CliOption("--cluster-identifier")] string ClusterIdentifier,
[property: CliOption("--revision-target")] string RevisionTarget
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}