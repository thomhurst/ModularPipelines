using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("quicksight", "delete-role-membership")]
public record AwsQuicksightDeleteRoleMembershipOptions(
[property: CliOption("--member-name")] string MemberName,
[property: CliOption("--role")] string Role,
[property: CliOption("--aws-account-id")] string AwsAccountId,
[property: CliOption("--namespace")] string Namespace
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}