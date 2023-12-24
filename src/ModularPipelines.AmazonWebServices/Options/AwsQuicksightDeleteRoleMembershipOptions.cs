using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "delete-role-membership")]
public record AwsQuicksightDeleteRoleMembershipOptions(
[property: CommandSwitch("--member-name")] string MemberName,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--namespace")] string Namespace
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}