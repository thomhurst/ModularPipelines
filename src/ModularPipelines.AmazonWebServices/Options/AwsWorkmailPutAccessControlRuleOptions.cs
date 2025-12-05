using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "put-access-control-rule")]
public record AwsWorkmailPutAccessControlRuleOptions(
[property: CliOption("--name")] string Name,
[property: CliOption("--effect")] string Effect,
[property: CliOption("--description")] string Description,
[property: CliOption("--organization-id")] string OrganizationId
) : AwsOptions
{
    [CliOption("--ip-ranges")]
    public string[]? IpRanges { get; set; }

    [CliOption("--not-ip-ranges")]
    public string[]? NotIpRanges { get; set; }

    [CliOption("--actions")]
    public string[]? Actions { get; set; }

    [CliOption("--not-actions")]
    public string[]? NotActions { get; set; }

    [CliOption("--user-ids")]
    public string[]? UserIds { get; set; }

    [CliOption("--not-user-ids")]
    public string[]? NotUserIds { get; set; }

    [CliOption("--impersonation-role-ids")]
    public string[]? ImpersonationRoleIds { get; set; }

    [CliOption("--not-impersonation-role-ids")]
    public string[]? NotImpersonationRoleIds { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}