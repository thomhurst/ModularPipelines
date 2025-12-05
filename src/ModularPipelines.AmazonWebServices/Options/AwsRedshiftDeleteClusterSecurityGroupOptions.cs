using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "delete-cluster-security-group")]
public record AwsRedshiftDeleteClusterSecurityGroupOptions(
[property: CliOption("--cluster-security-group-name")] string ClusterSecurityGroupName
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}