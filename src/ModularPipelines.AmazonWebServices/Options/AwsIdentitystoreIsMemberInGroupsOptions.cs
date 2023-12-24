using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("identitystore", "is-member-in-groups")]
public record AwsIdentitystoreIsMemberInGroupsOptions(
[property: CommandSwitch("--identity-store-id")] string IdentityStoreId,
[property: CommandSwitch("--member-id")] string MemberId,
[property: CommandSwitch("--group-ids")] string[] GroupIds
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}