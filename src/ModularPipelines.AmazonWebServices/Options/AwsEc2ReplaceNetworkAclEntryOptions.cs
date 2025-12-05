using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "replace-network-acl-entry")]
public record AwsEc2ReplaceNetworkAclEntryOptions(
[property: CliOption("--network-acl-id")] string NetworkAclId,
[property: CliOption("--protocol")] string Protocol,
[property: CliOption("--rule-action")] string RuleAction,
[property: CliOption("--rule-number")] int RuleNumber
) : AwsOptions
{
    [CliOption("--cidr-block")]
    public string? CidrBlock { get; set; }

    [CliOption("--icmp-type-code")]
    public string? IcmpTypeCode { get; set; }

    [CliOption("--ipv6-cidr-block")]
    public string? Ipv6CidrBlock { get; set; }

    [CliOption("--port-range")]
    public string? PortRange { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}