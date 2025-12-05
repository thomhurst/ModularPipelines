using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "authorize-endpoint-access")]
public record AwsRedshiftAuthorizeEndpointAccessOptions(
[property: CliOption("--account")] string Account
) : AwsOptions
{
    [CliOption("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CliOption("--vpc-ids")]
    public string[]? VpcIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}