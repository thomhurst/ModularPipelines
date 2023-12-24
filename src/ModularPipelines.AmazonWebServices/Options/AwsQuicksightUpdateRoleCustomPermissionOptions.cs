using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("quicksight", "update-role-custom-permission")]
public record AwsQuicksightUpdateRoleCustomPermissionOptions(
[property: CommandSwitch("--custom-permissions-name")] string CustomPermissionsName,
[property: CommandSwitch("--role")] string Role,
[property: CommandSwitch("--aws-account-id")] string AwsAccountId,
[property: CommandSwitch("--namespace")] string Namespace
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}