using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("redshift", "revoke-cluster-security-group-ingress")]
public record AwsRedshiftRevokeClusterSecurityGroupIngressOptions(
[property: CommandSwitch("--cluster-security-group-name")] string ClusterSecurityGroupName
) : AwsOptions
{
    [CommandSwitch("--cidrip")]
    public string? Cidrip { get; set; }

    [CommandSwitch("--ec2-security-group-name")]
    public string? Ec2SecurityGroupName { get; set; }

    [CommandSwitch("--ec2-security-group-owner-id")]
    public string? Ec2SecurityGroupOwnerId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}