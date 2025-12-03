using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-cluster-subnet-group")]
public record AwsRedshiftCreateClusterSubnetGroupOptions(
[property: CliOption("--cluster-subnet-group-name")] string ClusterSubnetGroupName,
[property: CliOption("--description")] string Description,
[property: CliOption("--subnet-ids")] string[] SubnetIds
) : AwsOptions
{
    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}