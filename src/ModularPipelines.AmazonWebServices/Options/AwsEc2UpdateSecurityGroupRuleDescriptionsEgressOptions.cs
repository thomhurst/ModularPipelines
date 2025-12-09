using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "update-security-group-rule-descriptions-egress")]
public record AwsEc2UpdateSecurityGroupRuleDescriptionsEgressOptions : AwsOptions
{
    [CliOption("--group-id")]
    public string? GroupId { get; set; }

    [CliOption("--group-name")]
    public string? GroupName { get; set; }

    [CliOption("--ip-permissions")]
    public string[]? IpPermissions { get; set; }

    [CliOption("--security-group-rule-descriptions")]
    public string[]? SecurityGroupRuleDescriptions { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}