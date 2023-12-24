using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("ec2", "replace-network-acl-association")]
public record AwsEc2ReplaceNetworkAclAssociationOptions(
[property: CommandSwitch("--association-id")] string AssociationId,
[property: CommandSwitch("--network-acl-id")] string NetworkAclId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}