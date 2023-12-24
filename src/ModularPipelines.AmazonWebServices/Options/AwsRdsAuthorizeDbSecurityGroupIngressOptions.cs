using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("rds", "authorize-db-security-group-ingress")]
public record AwsRdsAuthorizeDbSecurityGroupIngressOptions(
[property: CommandSwitch("--db-security-group-name")] string DbSecurityGroupName
) : AwsOptions
{
    [CommandSwitch("--cidrip")]
    public string? Cidrip { get; set; }

    [CommandSwitch("--ec2-security-group-name")]
    public string? Ec2SecurityGroupName { get; set; }

    [CommandSwitch("--ec2-security-group-id")]
    public string? Ec2SecurityGroupId { get; set; }

    [CommandSwitch("--ec2-security-group-owner-id")]
    public string? Ec2SecurityGroupOwnerId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}