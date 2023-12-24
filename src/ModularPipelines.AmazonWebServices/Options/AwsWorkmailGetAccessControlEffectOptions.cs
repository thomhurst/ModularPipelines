using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "get-access-control-effect")]
public record AwsWorkmailGetAccessControlEffectOptions(
[property: CommandSwitch("--organization-id")] string OrganizationId,
[property: CommandSwitch("--ip-address")] string IpAddress,
[property: CommandSwitch("--action")] string Action
) : AwsOptions
{
    [CommandSwitch("--user-id")]
    public string? UserId { get; set; }

    [CommandSwitch("--impersonation-role-id")]
    public string? ImpersonationRoleId { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}