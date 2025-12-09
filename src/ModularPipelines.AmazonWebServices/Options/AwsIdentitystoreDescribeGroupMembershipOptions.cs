using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identitystore", "describe-group-membership")]
public record AwsIdentitystoreDescribeGroupMembershipOptions(
[property: CliOption("--identity-store-id")] string IdentityStoreId,
[property: CliOption("--membership-id")] string MembershipId
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}