using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("redshift", "authorize-cluster-security-group-ingress")]
public record AwsRedshiftAuthorizeClusterSecurityGroupIngressOptions(
[property: CliOption("--cluster-security-group-name")] string ClusterSecurityGroupName
) : AwsOptions
{
    [CliOption("--cidrip")]
    public string? Cidrip { get; set; }

    [CliOption("--ec2-security-group-name")]
    public string? Ec2SecurityGroupName { get; set; }

    [CliOption("--ec2-security-group-owner-id")]
    public string? Ec2SecurityGroupOwnerId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}