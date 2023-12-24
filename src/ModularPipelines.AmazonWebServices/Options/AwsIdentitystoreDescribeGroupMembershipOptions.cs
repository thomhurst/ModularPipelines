using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identitystore", "describe-group-membership")]
public record AwsIdentitystoreDescribeGroupMembershipOptions(
[property: CommandSwitch("--identity-store-id")] string IdentityStoreId,
[property: CommandSwitch("--membership-id")] string MembershipId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}