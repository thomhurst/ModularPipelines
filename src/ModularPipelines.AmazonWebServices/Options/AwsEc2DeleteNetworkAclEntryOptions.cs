using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "delete-network-acl-entry")]
public record AwsEc2DeleteNetworkAclEntryOptions(
[property: CliOption("--network-acl-id")] string NetworkAclId,
[property: CliOption("--rule-number")] int RuleNumber
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}