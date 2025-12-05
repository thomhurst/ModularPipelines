using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "revoke-endpoint-access")]
public record AwsRedshiftRevokeEndpointAccessOptions : AwsOptions
{
    [CliOption("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CliOption("--account")]
    public string? Account { get; set; }

    [CliOption("--vpc-ids")]
    public string[]? VpcIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}