using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("rds", "authorize-db-security-group-ingress")]
public record AwsRdsAuthorizeDbSecurityGroupIngressOptions(
[property: CliOption("--db-security-group-name")] string DbSecurityGroupName
) : AwsOptions
{
    [CliOption("--cidrip")]
    public string? Cidrip { get; set; }

    [CliOption("--ec2-security-group-name")]
    public string? Ec2SecurityGroupName { get; set; }

    [CliOption("--ec2-security-group-id")]
    public string? Ec2SecurityGroupId { get; set; }

    [CliOption("--ec2-security-group-owner-id")]
    public string? Ec2SecurityGroupOwnerId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}