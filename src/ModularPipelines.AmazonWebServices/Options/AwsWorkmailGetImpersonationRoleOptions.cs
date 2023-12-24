using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "get-impersonation-role")]
public record AwsWorkmailGetImpersonationRoleOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--impersonation-role-id")] string ImpersonationRoleId
) : AwsOptions
{
    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}