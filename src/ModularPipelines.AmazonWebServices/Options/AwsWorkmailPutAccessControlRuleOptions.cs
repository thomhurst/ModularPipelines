using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CommandPrecedingArguments("workmail", "put-access-control-rule")]
public record AwsWorkmailPutAccessControlRuleOptions(
[property: CommandSwitch("--name")] string Name,
[property: CommandSwitch("--effect")] string Effect,
[property: CommandSwitch("--description")] string Description,
[property: CommandSwitch("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CommandSwitch("--ip-ranges")]
    public string[]? IpRanges { get; set; }

    [CommandSwitch("--not-ip-ranges")]
    public string[]? NotIpRanges { get; set; }

    [CommandSwitch("--actions")]
    public string[]? Actions { get; set; }

    [CommandSwitch("--not-actions")]
    public string[]? NotActions { get; set; }

    [CommandSwitch("--user-ids")]
    public string[]? UserIds { get; set; }

    [CommandSwitch("--not-user-ids")]
    public string[]? NotUserIds { get; set; }

    [CommandSwitch("--impersonation-role-ids")]
    public string[]? ImpersonationRoleIds { get; set; }

    [CommandSwitch("--not-impersonation-role-ids")]
    public string[]? NotImpersonationRoleIds { get; set; }

    [CommandSwitch("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}