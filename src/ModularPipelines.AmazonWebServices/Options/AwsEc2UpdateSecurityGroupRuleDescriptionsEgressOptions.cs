using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "update-security-group-rule-descriptions-egress")]
public record AwsEc2UpdateSecurityGroupRuleDescriptionsEgressOptions : AwsOptions
{
    [CommandSwitch("--group-id")]
    public string? GroupId { get; set; }

    [CommandSwitch("--group-name")]
    public string? GroupName { get; set; }

    [CommandSwitch("--ip-permissions")]
    public string[]? IpPermissions { get; set; }

    [CommandSwitch("--security-group-rule-descriptions")]
    public string[]? SecurityGroupRuleDescriptions { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}