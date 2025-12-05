using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "delete-cluster-subnet-group")]
public record AwsRedshiftDeleteClusterSubnetGroupOptions(
[property: CliOption("--cluster-subnet-group-name")] string ClusterSubnetGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}