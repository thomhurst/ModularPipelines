using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("identitystore", "is-member-in-groups")]
public record AwsIdentitystoreIsMemberInGroupsOptions(
[property: CliOption("--identity-store-id")] string IdentityStoreId,
[property: CliOption("--member-id")] string MemberId,
[property: CliOption("--group-ids")] string[] GroupIds
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}