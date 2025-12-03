using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identitystore", "get-group-membership-id")]
public record AwsIdentitystoreGetGroupMembershipIdOptions(
[property: CliOption("--identity-store-id")] string IdentityStoreId,
[property: CliOption("--group-id")] string GroupId,
[property: CliOption("--member-id")] string MemberId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}