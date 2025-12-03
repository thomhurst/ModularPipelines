using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("elasticache", "revoke-cache-security-group-ingress")]
public record AwsElasticacheRevokeCacheSecurityGroupIngressOptions(
[property: CliOption("--cache-security-group-name")] string CacheSecurityGroupName,
[property: CliOption("--ec2-security-group-name")] string Ec2SecurityGroupName,
[property: CliOption("--ec2-security-group-owner-id")] string Ec2SecurityGroupOwnerId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}