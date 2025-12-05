using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "modify-db-proxy-endpoint")]
public record AwsRdsModifyDbProxyEndpointOptions(
[property: CliOption("--db-proxy-endpoint-name")] string DbProxyEndpointName
) : AwsOptions
{
    [CliOption("--new-db-proxy-endpoint-name")]
    public string? NewDbProxyEndpointName { get; set; }

    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}