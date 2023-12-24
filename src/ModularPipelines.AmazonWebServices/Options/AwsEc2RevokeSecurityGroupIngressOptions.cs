using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "revoke-security-group-ingress")]
public record AwsEc2RevokeSecurityGroupIngressOptions : AwsOptions
{
    [CommandSwitch("--group-id")]
    public string? GroupId { get; set; }

    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--ip-permissions")]
    public string[]? IpPermissions { get; set; }

    [CommandSwitch("--security-group-rule-ids")]
    public string[]? SecurityGroupRuleIds { get; set; }

    [CommandSwitch("--protocol")]
    public string? Protocol { get; set; }

    [CommandSwitch("--port")]
    public string? Port { get; set; }

    [CommandSwitch("--cidr")]
    public string? Cidr { get; set; }

    [CommandSwitch("--source-group")]
    public string? SourceGroup { get; set; }

    [CommandSwitch("--group-owner")]
    public string? GroupOwner { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}