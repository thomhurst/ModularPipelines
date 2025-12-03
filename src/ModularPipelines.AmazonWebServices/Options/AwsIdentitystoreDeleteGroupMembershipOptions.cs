using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identitystore", "delete-group-membership")]
public record AwsIdentitystoreDeleteGroupMembershipOptions(
[property: CliOption("--identity-store-id")] string IdentityStoreId,
[property: CliOption("--membership-id")] string MembershipId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}