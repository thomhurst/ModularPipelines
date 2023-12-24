using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "delete-network-acl-entry")]
public record AwsEc2DeleteNetworkAclEntryOptions(
[property: CommandSwitch("--network-acl-id")] string NetworkAclId,
[property: CommandSwitch("--rule-number")] int RuleNumber
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}