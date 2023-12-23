using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "replace-network-acl-entry")]
public record AwsEc2ReplaceNetworkAclEntryOptions(
[property: CommandSwitch("--network-acl-id")] string NetworkAclId,
[property: CommandSwitch("--protocol")] string Protocol,
[property: CommandSwitch("--rule-action")] string RuleAction,
[property: CommandSwitch("--rule-number")] int RuleNumber
) : AwsOptions
{
    [CommandSwitch("--cidr-block")]
    public string? CidrBlock { get; set; }

    [CommandSwitch("--icmp-type-code")]
    public string? IcmpTypeCode { get; set; }

    [CommandSwitch("--ipv6-cidr-block")]
    public string? Ipv6CidrBlock { get; set; }

    [CommandSwitch("--port-range")]
    public string? PortRange { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}