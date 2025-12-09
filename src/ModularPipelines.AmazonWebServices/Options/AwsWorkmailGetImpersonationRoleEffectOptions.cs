using System.Diagnostics.CodeAnalysis;
using ModularPipelines.Attributes;

namespace ModularPipelines.AmazonWebServices.Options;

[ExcludeFromCodeCoverage]
[CliCommand("workmail", "get-impersonation-role-effect")]
public record AwsWorkmailGetImpersonationRoleEffectOptions(
[property: CliOption("--organization-id")] string OrganizationId,
[property: CliOption("--impersonation-role-id")] string ImpersonationRoleId,
[property: CliOption("--target-user")] string TargetUser
) : AwsOptions
{
    [CliOption("--generate-cli-skeleton")]
    public string? GenerateCliSkeleton { get; set; }
}