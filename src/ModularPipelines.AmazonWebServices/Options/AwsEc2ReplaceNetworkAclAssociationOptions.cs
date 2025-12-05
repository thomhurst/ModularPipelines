using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("ec2", "replace-network-acl-association")]
public record AwsEc2ReplaceNetworkAclAssociationOptions(
[property: CliOption("--association-id")] string AssociationId,
[property: CliOption("--network-acl-id")] string NetworkAclId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}