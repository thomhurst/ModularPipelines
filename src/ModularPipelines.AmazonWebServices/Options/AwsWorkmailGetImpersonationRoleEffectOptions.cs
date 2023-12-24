using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "get-impersonation-role-effect")]
public record AwsWorkmailGetImpersonationRoleEffectOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--impersonation-role-id")] string ImpersonationRoleId,
[property: CommandSwitch("--target-user")] string TargetUser
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}