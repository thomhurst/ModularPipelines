using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "get-cluster-credentials-with-iam")]
public record AwsRedshiftGetClusterCredentialsWithIamOptions : AwsOptions
{
    [CliOption("--db-name")]
    public string? DbName { get; set; }

    [CliOption("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CliOption("--duration-seconds")]
    public int? DurationSeconds { get; set; }

    [CliOption("--custom-domain-name")]
    public string? CustomDomainName { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}