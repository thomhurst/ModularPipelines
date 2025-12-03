using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "create-db-proxy")]
public record AwsRdsCreateDbProxyOptions(
[property: CliOption("--db-proxy-name")] string DbProxyName,
[property: CliOption("--engine-family")] string EngineFamily,
[property: CliOption("--auth")] string[] Auth,
[property: CliOption("--role-arn")] string RoleArn,
[property: CliOption("--vpc-subnet-ids")] string[] VpcSubnetIds
) : AwsOptions
{
    [CliOption("--vpc-security-group-ids")]
    public string[]? VpcSecurityGroupIds { get; set; }

    [CliOption("--idle-client-timeout")]
    public int? IdleClientTimeout { get; set; }

    [CliOption("--tags")]
    public string[]? Tags { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}