using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-db-proxy-endpoint")]
public record AwsRdsCreateDbProxyEndpointOptions(
[property: CliOption("--db-proxy-name")] string DbProxyName,
[property: CliOption("--db-proxy-endpoint-name")] string DbProxyEndpointName,
[property: CliOption("--vpc-subnet-ids")] string[] VpcSubnetIds
) : AwsOptions
{
    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--target-role")]
    public string? TargetRole { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}