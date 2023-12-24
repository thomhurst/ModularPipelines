using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identitystore", "get-group-membership-id")]
public record AwsIdentitystoreGetGroupMembershipIdOptions(
[property: CommandSwitch("--identity-store-id")] string IdentityStoreId,
[property: CommandSwitch("--group-id")] string GroupId,
[property: CommandSwitch("--member-id")] string MemberId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}