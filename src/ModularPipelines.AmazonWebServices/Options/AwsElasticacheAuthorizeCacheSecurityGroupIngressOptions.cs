using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("elasticache", "authorize-cache-security-group-ingress")]
public record AwsElasticacheAuthorizeCacheSecurityGroupIngressOptions(
[property: CommandSwitch("--cache-security-group-name")] string CacheSecurityGroupName,
[property: CommandSwitch("--ec2-security-group-name")] string Ec2SecurityGroupName,
[property: CommandSwitch("--ec2-security-group-owner-id")] string Ec2SecurityGroupOwnerId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}