using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "revoke-security-group-egress")]
public record AwsEc2RevokeSecurityGroupEgressOptions(
[property: CliOption("--group-id")] string GroupId
) : AwsOptions
{
    [CliOption("--ip-permissions")]
    public string[]? IpPermissions { get; set; }

    [CliOption("--security-group-rule-ids")]
    public string[]? SecurityGroupRuleIds { get; set; }

    [CliOption("--protocol")]
    public string? Protocol { get; set; }

    [CliOption("--port")]
    public string? Port { get; set; }

    [CliOption("--cidr")]
    public string? Cidr { get; set; }

    [CliOption("--source-group")]
    public string? SourceGroup { get; set; }

    [CliOption("--group-owner")]
    public string? GroupOwner { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}