using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "get-access-control-effect")]
public record AwsWorkmailGetAccessControlEffectOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--ip-address")] string IpAddress,
[property: CliOption("--action")] string Action
) : AwsOptions
{
    [CliOption("--user-id")]
    public string? UserId { get; set; }

    [CliOption("--impersonation-role-id")]
    public string? ImpersonationRoleId { get; set; }

    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}