using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "create-endpoint-access")]
public record AwsRedshiftCreateEndpointAccessOptions(
[property: CliOption("--endpoint-name")] string EndpointName,
[property: CliOption("--subnet-group-name")] string SubnetGroupName
) : AwsOptions
{
    [CliOption("--cluster-identifier")]
    public string? ClusterIdentifier { get; set; }

    [CliOption("--resource-owner")]
    public string? ResourceOwner { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}